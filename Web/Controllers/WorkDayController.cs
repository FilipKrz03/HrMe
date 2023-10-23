using Application.CQRS;
using Application.CQRS.WorkDay.Command.CreateWorkDay;
using Application.CQRS.WorkDay.Response;
using Domain.Abstractions;
using MediatR;
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

        [HttpPost]
        public async Task<ActionResult<Response<WorkDayResponse?>>> CreateEmployeeWorkDay(Guid employeeId , CreateWorkDayRequest request)
        {
            var comapnyId = _userService.GetUserId();

            CreateWorkDayCommand command = 
                new(comapnyId, employeeId, request.WorkDayDate, request.StartTime, request.EndTime);

            Response<WorkDayResponse?> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                   StatusCode(201, result.Value);
        }
    }
}
