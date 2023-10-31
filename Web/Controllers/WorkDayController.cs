using Application.CQRS;
using Application.CQRS.WorkDay.Command.CreateWorkDay;
using Application.CQRS.WorkDay.Query.GetWorkDay;
using Application.CQRS.WorkDay.Query.GetWorkDays;
using Domain.Responses;
using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/workdays")]
    [ApiController]
    [Authorize]

    public class WorkDayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public WorkDayController(IMediator mediator , IUserService userService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{workDayId}" , Name = "GetWorkDay")]
        public async Task<ActionResult<Response<WorkDayResponse>>> GetEmployeeWorkDay(Guid employeeId, Guid workDayId)
        {
            var companyId = _userService.GetUserId();

            GetWorkDayQuery query = new (companyId , employeeId , workDayId);

            Response<WorkDayResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  Ok(result.Value);
        }
    
        [HttpPost]
        public async Task<ActionResult<Response<WorkDayResponse>>> CreateEmployeeWorkDay(Guid employeeId , CreateWorkDayRequest request)
        {
            var comapnyId = _userService.GetUserId();

            CreateWorkDayCommand command = 
                new(comapnyId, employeeId, request.WorkDayDate, request.StartTime, request.EndTime);

            Response<WorkDayResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  CreatedAtRoute("GetWorkDay", new
                  {
                      employeeId , 
                      workDayId = result.Value!.Id
                  } , result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<Response<PagedList<WorkDayResponse>>>> GetWorkDays
            (Guid employeeId , [FromQuery] ResourceParameters resourceParameters)
        {
            var companyId = _userService.GetUserId();

            GetWorkDaysQuery query = new(companyId , employeeId , resourceParameters);

            Response<PagedList<WorkDayResponse>> result = await _mediator.Send(query);

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
    }
}
