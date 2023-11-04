using Application.CQRS;
using Application.CQRS.WageQueries.GetWageForMonth;
using Domain.Abstractions;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

    // Wage is not entity , it is calculate from employee work days and contract info 
    // So only get actions are allowed

    [Route("api/employees/{employeeId}/wages")]
    [ApiController]
    [Authorize]
    public class WageController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public WageController(IMediator mediator , IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        //[HttpGet]
        //public async Task GetWages()
        //{

        //}

        [HttpGet("{year}/{monthNumber}")]
        public async Task<ActionResult<Response<WageResponse>>>
            GetWageForMonth(Guid employeeId , int year , int monthNumber)
        {
            Guid companyId = _userService.GetUserId();

            GetWageForMonthQuery query = new(companyId , employeeId , monthNumber , year);

            Response<WageResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }



    }
}
