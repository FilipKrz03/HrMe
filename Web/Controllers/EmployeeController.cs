using Application.CQRS;
using Application.CQRS.Company.Response;
using Application.CQRS.Employee.Command.CreateEmployee;
using Application.CQRS.Employee.Query.GetEmployee;
using Application.CQRS.Employee.Query.GetEmployees;
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
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        [HttpGet("{employeeId}" , Name = "GetEmployee")]
        [Authorize]
        public async Task<ActionResult<Response<EmployeeResponse?>>> GetEmployee(Guid employeeId)
        {
            var companyId = Guid.Parse(User.FindFirstValue(ClaimTypes.PrimarySid));

            GetEmployeeQuery query = new(employeeId , companyId);

            Response<EmployeeResponse?> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
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
                : CreatedAtRoute("GetEmployee",
                new
                {
                    employeeId = result.Value!.Id
                } ,
                result.Value);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<EmployeeResponse>?>>> GetEmployees()
        {
            var companyGuid = Guid.Parse(User.FindFirstValue(ClaimTypes.PrimarySid));

            GetEmployeesQuery query = new(companyGuid);

            var result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }
    }
}
