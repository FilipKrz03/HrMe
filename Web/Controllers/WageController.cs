using Application.CQRS;
using Application.CQRS.WageQueries.GetEmployeesWagesForMonth;
using Application.CQRS.WageQueries.GetWageForMonth;
using Domain.Abstractions;
using Domain.Common;
using Domain.Responses;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Controllers
{

    // Wage is not entity , it is calculate from employee work days and contract info 
    // So only get actions are allowed

    [Route("api/employees")]
    [ApiController]
    [Authorize]
    public class WageController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public WageController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpGet("wages/{year}/{monthNumber}")]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<PagedList<WageResponse>>>>
            GetWagesForEmployees(int year, int monthNumber, [FromQuery] ResourceParameters resourceParameters)
        {
            var companyId = _userService.GetCompanyId();

            GetEmployeesWagesForMonthQuery query =
                new(resourceParameters, companyId, year, monthNumber);

            Response<PagedList<WageResponse>> result = await _mediator.Send(query);

            if (result.IsError)
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

        [HttpGet("{employeeId}/wages/{year}/{monthNumber}")]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<WageResponse>>>
            GetWageForMonth(Guid employeeId, int year, int monthNumber)
        {
            Guid companyId = _userService.GetCompanyId();

            GetWageForMonthQuery query = new(companyId, employeeId, monthNumber, year);

            Response<WageResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }

        [HttpGet]
        [Route("~/api/employee/wages/{year}/{monthNumber}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Response<WageResponse>>>
            GetWageForMonthByEmployee(int year, int monthNumber)
        {
            var employeeId = _userService.GetEmployeeId();

            return await GetWageForMonth(employeeId, year, monthNumber);
        }
    }
}
