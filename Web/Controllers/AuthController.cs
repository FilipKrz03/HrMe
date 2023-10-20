using Application.Company.Command.CreateCompany;
using Application.Company.Command.LoginCompany;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCompany(CreateCompanyCommand command)
        {
            var result = await _mediator.Send(command);

            return result == null ? StatusCode(409, $"Company with email {command.Email} already exists") :
                 StatusCode(201, "Company created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCompany(LoginCompanyCommand command)
        {
            var result = await _mediator.Send(command);

            return result == null ? Unauthorized("Check email or password") :
                Ok(result);
        }
    }
}
