using Application.CQRS;
using Application.CQRS.Company.Command.CreateCompany;
using Application.CQRS.MonthlyBonus.Command;
using Application.CQRS.MonthlyBonus.Command.CreateMonthlyBonus;
using Application.CQRS.MonthlyBonus.Command.DeleteMonthlyBonus;
using Application.CQRS.MonthlyBonus.Query.GetMonthlyBonus;
using Application.CQRS.MonthlyBonus.Query.GetMonthlyBonuses;
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
    [Route("api/employees/{employeeId}/monthlybonuses")]
    [ApiController]
    [Authorize]
    public class MonthlyBonusesController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public MonthlyBonusesController(IUserService userService, IMediator mediator)
        {
            _mediator = mediator;
            _userService = userService;
        }

        [HttpGet("{monthlyBonusId}", Name = "GetMonthlyBonus")]
        public async Task<ActionResult<Response<MonthlyBonusResponse>>>
            GetMonthlyBonus(Guid employeeId, Guid monthlyBonusId)
        {
            var companyId = _userService.GetUserId();

            GetMonthlyBonusCommand command = new(monthlyBonusId, companyId, employeeId);

            Response<MonthlyBonusResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Response<MonthlyBonusResponse>>>
            CreateMonthlyBonus(Guid employeeId, MonthlyBonusRequest request)
        {
            var companyId = _userService.GetUserId();

            CreateMonthlyBonusCommand command
                = new(companyId, employeeId, request.Year, request.Month, request.BonusAmount, request.BonusInPercent);

            Response<MonthlyBonusResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : CreatedAtRoute("GetMonthlyBonus", new
                {
                    employeeId,
                    monthlyBonusId = result.Value!.Id
                }, result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<Response<PagedList<MonthlyBonusResponse>>>>
            GetMonthlyBonuses(Guid employeeId, [FromQuery] ResourceParameters resourceParameters)
        {
            var companyId = _userService.GetUserId();

            GetMonthlyBonusesQuery query = new(companyId, employeeId, resourceParameters);

            Response<PagedList<MonthlyBonusResponse>> result = await _mediator.Send(query);

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

        [HttpDelete("{monthlyBonusId}")]
        public async Task<ActionResult<Response<bool>>>
            DeleteMonthlyBonus(Guid employeeId, Guid monthlyBonusId)
        {
            var companyId = _userService.GetUserId();

            DeleteMonthlyBonusCommand command = new(companyId, employeeId, monthlyBonusId);

            Response<bool> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
               : NoContent();
        }
    }
}
