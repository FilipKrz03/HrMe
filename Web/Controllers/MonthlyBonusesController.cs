using Application.CQRS;
using Application.CQRS.Company.Command.CreateCompany;
using Application.CQRS.MonthlyBonus.Command;
using Application.CQRS.MonthlyBonus.Command.CreateMonthlyBonus;
using Domain.Abstractions;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/monthlybonuses")]
    [ApiController]
    [Authorize]
    public class MonthlyBonusesController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public MonthlyBonusesController(IUserService userService, IMediator mediator)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<MonthlyBonusResponse>>>
            CreateMonthlyBonus(Guid employeeId, MonthlyBonusRequest request)
        {
            var companyId = _userService.GetUserId();

            CreateMonthlyBonusCommand command
                = new(companyId, employeeId, request.Year, request.Month, request.BonusAmount, request.BonusInPercent);

            Response<MonthlyBonusResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : StatusCode(201);
        }
    }
}
