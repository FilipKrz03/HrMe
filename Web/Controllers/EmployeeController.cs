using Application.CQRS;
using Application.CQRS.Employee.Command.CreateEmployee;
using Application.CQRS.Employee.Query.GetEmployee;
using Application.CQRS.Employee.Query.GetEmployees;
using Domain.Responses;
using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Web.Services;
using Application.CQRS.Employee.Command.DeleteEmployee;
using Application.CQRS.Employee.Command;
using Application.CQRS.Employee.Command.PutEmployee;

namespace Web.Controllers
{
    [Route("api/employees")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        public EmployeeController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator ?? throw new ArgumentException(nameof(mediator));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{employeeId}", Name = "GetEmployee")]

        public async Task<ActionResult<Response<EmployeeResponse>>> GetEmployee(Guid employeeId)
        {
            var companyGuid = _userService.GetUserId();

            GetEmployeeQuery query = new(employeeId, companyGuid);

            Response<EmployeeResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Response<EmployeeResponse>>> CreateEmployee(EmployeeRequest request)
        {
            var companyGuid = _userService.GetUserId();

            CreateEmployeeCommand command = new(companyGuid, request.FirstName, request.LastName
                , request.Position, request.Email, request.DateOfBirth);

            Response<EmployeeResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : CreatedAtRoute("GetEmployee",
                new
                {
                    employeeId = result.Value!.Id
                },
                result.Value);
        }

        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<ActionResult<Response<PagedList<EmployeeResponse>>>>
            GetEmployees([FromQuery] ResourceParameters resourceParameters)
        {
            var companyGuid = _userService.GetUserId();

            GetEmployeesQuery query = new(companyGuid, resourceParameters);

            Response<PagedList<EmployeeResponse>> result = await _mediator.Send(query);

            if (result.IsError == true)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            var paginationMetadata = new
            {
                totalCount = result.Value!.TotalCount,
                pageSize = result.Value!.PageSize,
                pageNumber = result.Value!.PageNumber,
                hasPrevious = result.Value!.HasPrevios,
                hasNext = result.Value!.HasNext,
                totalPages = result.Value!.TotalPages,
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            return Ok(result.Value);
        }

        [HttpDelete("{employeeId}")]
        public async Task<ActionResult<Response<bool>>> DeleteEmployee(Guid employeeId)
        {
            var companyGuid = _userService.GetUserId();

            DeleteEmployeeCommand command = new(companyGuid, employeeId);

            Response<bool> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : NoContent();
        }

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<Response<EmployeeResponse>>>
            PutEmployee(Guid employeeId, EmployeeRequest request)
        {
            var companyGuid = _userService.GetUserId();

            PutEmployeeComand command = new(companyGuid, employeeId, request.FirstName, request.LastName,
                request.Position, request.Email, request.DateOfBirth);

            Response<EmployeeResponse> result = await _mediator.Send(command);

            if (result.IsError == true)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            if (result.Value != null)
            {
                return CreatedAtRoute("GetEmployee",
                    new
                    {
                        employeeId = result.Value!.Id
                    },
                    result.Value);
            }

            return NoContent();
        }
    }
}
