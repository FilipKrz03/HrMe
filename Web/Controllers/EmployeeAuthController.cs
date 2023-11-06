using Application.CQRS;
using Application.CQRS.Employee.Command.LoginEmployee;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/employees/authentication")]
    [ApiController]
    public class EmployeeAuthController : ControllerBase
    {

        private IMediator _mediator;

        public EmployeeAuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>>
            LoginEmployee(LoginEmployeeCommand command)
        {
            Response<string> result = await _mediator.Send(command);

            return result.IsError ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }
    }
}
