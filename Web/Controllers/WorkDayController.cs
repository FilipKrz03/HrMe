using Application.CQRS;
using Application.CQRS.WorkDay.Command.CreateWorkDay;
using Application.CQRS.WorkDay.Query.GetWorkDay;
using Application.CQRS.WorkDay.Response;
using Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/workdays")]
    [ApiController]

    public class WorkDayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public WorkDayController(IMediator mediator , IUserService userService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [Authorize]
        [HttpGet("{workDayId}" , Name = "GetWorkDay")]
        public async Task<ActionResult<Response<WorkDayResponse>>> GetEmployeeWorkDay(Guid employeeId, Guid workDayId)
        {
            var companyId = _userService.GetUserId();

            GetWorkDayQuery query = new GetWorkDayQuery(companyId , employeeId , workDayId);

            Response <WorkDayResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  Ok(result.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Response<WorkDayResponse?>>> CreateEmployeeWorkDay(Guid employeeId , CreateWorkDayRequest request)
        {
            var comapnyId = _userService.GetUserId();

            CreateWorkDayCommand command = 
                new(comapnyId, employeeId, request.WorkDayDate, request.StartTime, request.EndTime);

            Response<WorkDayResponse?> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  CreatedAtRoute("GetWorkDay", new
                  {
                      employeeId , 
                      workDayId = result.Value!.Id
                  } , result.Value);
        }
    }
}
