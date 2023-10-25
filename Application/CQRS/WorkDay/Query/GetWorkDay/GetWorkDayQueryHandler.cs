using Application.CQRS.WorkDay.Response;
using AutoMapper;
using Domain.Abstractions;
using Domain.Common.Responses;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.WorkDay.Query.GetWorkDay
{
    public class GetWorkDayQueryHandler : IRequestHandler<GetWorkDayQuery, Response<WorkDayResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IComapniesContextRepostiory _companiesContextRepository;

        public GetWorkDayQueryHandler
            (IMapper mapper, IComapniesContextRepostiory comapniesContextRepostiory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companiesContextRepository = comapniesContextRepostiory ??
                throw new ArgumentNullException(nameof(comapniesContextRepostiory));
        }

        public async Task<Response<WorkDayResponse>> Handle(GetWorkDayQuery request, CancellationToken cancellationToken)
        {
            Response<WorkDayResponse> response = new();

            EmployeAndCompanyExist exist = await _companiesContextRepository
                .EmployeAndCompanyExistAsync(request.CompanyId, request.EmployeeId);

            if (!exist.CompanyExist)
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!exist.EmployeeExist)
            {
                return
                    response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDay = await _companiesContextRepository
                .GetEmployeeWorkDayAsync(request.EmployeeId, request.WorkDayId);

            if (workDay == null)
            {
                return
                    response.SetError(404, $"We could not find work day with id {request.WorkDayId}");
            }

            response.Value = _mapper.Map<WorkDayResponse>(workDay);

            return response;
        }
    }
}
