using Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
