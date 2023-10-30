﻿using Application.CQRS;
using Application.CQRS.PaymentInfo.Command.CreatePaymentInfo;
using Application.CQRS.PaymentInfo.Query.GePaymentInfo;
using Application.CQRS.PaymentInfo.Query.GetPaymentInfos;
using Application.CQRS.PaymentInfo.Response;
using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web.Controllers
{
    [Route("api/employees/{employeeId}/paymentinfos")]
    [ApiController]
    [Authorize]
    public class PaymentInfoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public PaymentInfoController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator ??
                throw new ArgumentNullException(nameof(mediator));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{paymentInfoId}", Name = "GetPaymentInfo")]

        public async Task<ActionResult<Response<PaymentInfoResponse>>> GetPaymentInfo
           (Guid employeeId, Guid paymentInfoId)
        {
            var companyId = _userService.GetUserId();

            GetPaymentInfoQuery query = new
            (companyId, employeeId, paymentInfoId);

            Response<PaymentInfoResponse> result = await _mediator.Send(query);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Response<PaymentInfoResponse>>> CreatePaymentInfo(Guid employeeId, CreatePaymentInfoRequest request)
        {
            var companyId = _userService.GetUserId();

            CreatePaymentInfoCommand command = new(companyId, employeeId, request.HourlyRateBrutto,
                request.ContractType, request.StartOfContractDate, request.EndOfContractDate);

            Response<PaymentInfoResponse> result = await _mediator.Send(command);

            return result.IsError == true ? StatusCode(result.StatusCode, result.Message)
                : CreatedAtRoute("GetPaymentInfo", new
                {
                    employeeId,
                    paymentInfoId = result.Value!.Id
                }, result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<Response<PagedList<PaymentInfoResponse>>>>
            GetPaymentInfos(Guid employeeId , [FromQuery]ResourceParameters resourceParameters)
        {
            var companyId = _userService.GetUserId();

            GetPaymentInfosQuery query = new(companyId, employeeId , resourceParameters);

            Response<PagedList<PaymentInfoResponse>> result = await _mediator.Send(query);

            if(result.IsError == true)
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
