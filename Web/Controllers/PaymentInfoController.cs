using Application.CQRS;
using Application.CQRS.PaymentInfo.Command.CreatePaymentInfo;
using Application.CQRS.PaymentInfo.Response;
using Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/paymentinfos")]
    [ApiController]
    [Authorize]
    public class PaymentInfoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public PaymentInfoController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public async Task<ActionResult> CreatePaymentInfo(Guid employeeId, CreatePaymentInfoRequest request)
        {
            var companyId = _userService.GetUserId();

            CreatePaymentInfoCommand command = new(companyId, employeeId, request.HourlyRateBrutto,
                request.ContractType, request.StartOfContractDate, request.EndOfContractDate);

            Response<PaymentInfoResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }
    }
}
