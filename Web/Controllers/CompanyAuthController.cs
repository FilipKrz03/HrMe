using Application.CQRS;
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
    [Route("api/companies/authentication")]
    [ApiController]
    public class CompanyAuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyAuthController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<string?>>> RegisterCompany(CreateCompanyCommand command)
        {
            Response<string?> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                 StatusCode(201, result.Value);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<string?>>> LoginCompany(LoginCompanyCommand command)
        {
            Response<string?> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                 Ok(result.Value);
        }
    }
}
