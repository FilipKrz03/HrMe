using Application.CQRS.Employee.Command.CreateEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateEmployee(CreateEmployeeRequest request)
        {
            var companyGuid = User.FindFirstValue(ClaimTypes.PrimarySid);

            CreateEmployeeCommand command = new()
            {
                CompanyGuid = companyGuid,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Position = request.Position,
            };

            var result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }
    }
}
