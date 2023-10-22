using Application.CQRS;
using Application.CQRS.Company.Response;
using Application.CQRS.Employee.Command.CreateEmployee;
using Application.CQRS.Employee.Response;
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
        public async Task<ActionResult<Response<EmployeeResponse?>>> CreateEmployee(CreateEmployeeRequest request)
        {
            var companyGuid = User.FindFirstValue(ClaimTypes.PrimarySid);

            CreateEmployeeCommand command = new(companyGuid, request.FirstName, request.LastName
                , request.Position, request.Email, request.DateOfBirth);
           
            Response<EmployeeResponse?> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }
    }
}
