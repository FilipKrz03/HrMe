﻿using Application.CQRS;
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
using Application.CQRS.WorkDay.Command.DeleteWorkDay;
using Application.CQRS.WorkDay.Command;
using Application.CQRS.WorkDay.Command.PutWorkDay;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/workdays")]
    [ApiController]
    [Authorize]

    public class WorkDayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public WorkDayController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{workDayId}", Name = "GetWorkDay")]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<WorkDayResponse>>> GetEmployeeWorkDay(Guid employeeId, Guid workDayId)
        {
            var companyId = _userService.GetCompanyId();

            GetWorkDayQuery query = new(companyId, employeeId, workDayId);

            Response<WorkDayResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  Ok(result.Value);
        }

        [HttpGet]
        [Route("~/api/employee/workdays/{workDayId}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Response<WorkDayResponse>>> GetWorkDayByEmployee(Guid workDayId)
        {
            var employeeId = _userService.GetEmployeeId();

            return await GetEmployeeWorkDay(employeeId, workDayId);
        }

        [HttpPost]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<WorkDayResponse>>> CreateEmployeeWorkDay(Guid employeeId, WorkDayRequest request)
        {
            var comapnyId = _userService.GetCompanyId();

            CreateWorkDayCommand command =
                new(comapnyId, employeeId, request.WorkDayDate, request.StartTime, request.EndTime);

            Response<WorkDayResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message) :
                  CreatedAtRoute("GetWorkDay", new
                  {
                      employeeId,
                      workDayId = result.Value!.Id
                  }, result.Value);
        }

        [HttpGet]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<PagedList<WorkDayResponse>>>> GetWorkDays
            (Guid employeeId, [FromQuery] ResourceParameters resourceParameters)
        {
            var companyId = _userService.GetCompanyId();

            GetWorkDaysQuery query = new(companyId, employeeId, resourceParameters);

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

        [HttpGet]
        [Route("~/api/employee/workdays")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Response<PagedList<WorkDayResponse>>>> GetWorkDaysByEmployee
            ([FromQuery] ResourceParameters resourceParameters)
        {
            var employeeId = _userService.GetEmployeeId();

            return await GetWorkDays(employeeId, resourceParameters);
        }

        [HttpDelete("{workDayId}")]
        [Authorize(Roles = "Company")]
        public async Task<ActionResult<Response<bool>>> DeleteWorkDay(Guid employeeId, Guid workDayId)
        {
            var companyId = _userService.GetCompanyId();

            DeleteWorkDayCommand command = new(companyId, employeeId, workDayId);

            Response<bool> result = await _mediator.Send(command);

            return result.IsError ? StatusCode(result.StatusCode, result.Message)
                : NoContent();
        }

        [HttpPut("{workDayId}")]
        [Authorize(Roles = "Company")]

        public async Task<ActionResult<Response<WorkDayResponse>>>
            PutWorkDay(Guid employeeId, Guid workDayId, WorkDayRequest request)
        {
            var comapnyId = _userService.GetCompanyId();

            PutWorkDayCommand command = new(comapnyId, employeeId, workDayId, request.WorkDayDate,
                request.StartTime, request.EndTime);

            Response<WorkDayResponse> result = await _mediator.Send(command);

            if (result.IsError)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            if (result.Value != null)
            {
                return CreatedAtRoute("GetWorkDay", new
                {
                    employeeId,
                    workDayId = result.Value!.Id
                }, result.Value);
            }

            return NoContent();
        }
    }
}
