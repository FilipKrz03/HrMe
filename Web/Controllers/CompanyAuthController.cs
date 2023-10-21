using Application.CQRS.Company.Command.CreateCompany;
using Application.CQRS.Company.Command.LoginCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/company/authentication")]
    [ApiController]
    public class CompanyAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyAuthController(IMediator mediator)
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
