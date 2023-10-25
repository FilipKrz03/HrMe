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

namespace Application.CQRS.WorkDay.Query.GetWorkDays
{
    public class GetWorkDaysQueryHandler : IRequestHandler<GetWorkDaysQuery, Response<IEnumerable<WorkDayResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IComapniesContextRepostiory _companiesContextRepository;   

        public GetWorkDaysQueryHandler(HrMeContext context, IMapper mapper , IComapniesContextRepostiory comapniesContextRepostiory)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companiesContextRepository = comapniesContextRepostiory ??
                throw new ArgumentNullException(nameof(comapniesContextRepostiory));
        }

        public async Task<Response<IEnumerable<WorkDayResponse>>> Handle(GetWorkDaysQuery request, CancellationToken cancellationToken)
        {
            Response<IEnumerable<WorkDayResponse>> response = new();

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

            var workDays = await _companiesContextRepository.GetEmployeeWorkDaysAsync(request.EmployeeId);

            response.Value = _mapper.Map<IEnumerable<WorkDayResponse>>(workDays);

            return response;
        }
    }
}
