using Application.Company.Command.CreateCompany;
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
            if(command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result == null)
                return StatusCode(409, $"Company with email {command.Email} already exists");

            return StatusCode(201, "Company created");
        }
    }
}
