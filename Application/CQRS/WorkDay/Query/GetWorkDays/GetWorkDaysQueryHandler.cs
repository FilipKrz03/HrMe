using Domain.Responses;
using AutoMapper;
using Domain.Abstractions;
using Infrastructure;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.PropertyMapping;

namespace Application.CQRS.WorkDay.Query.GetWorkDays
{
    public class GetWorkDaysQueryHandler : IRequestHandler<GetWorkDaysQuery, Response<PagedList<WorkDayResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkDayReposiotry _workDayRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public GetWorkDaysQueryHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IWorkDayReposiotry workDayRepository,
            IPropertyMappingService propertyMappingService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _workDayRepository = workDayRepository;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<Response<PagedList<WorkDayResponse>>> Handle(GetWorkDaysQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<WorkDayResponse>> response = new();

            if (!_propertyMappingService.PropertyMappingExist
              <Domain.Entities.EmployeeWorkDay, WorkDayResponse>(request.ResourceParameters.OrderBy!))
            {
                return response.SetError(400, $"One of orderBy Fields that you entered does not exist : " +
                     $"{request.ResourceParameters.OrderBy}");
            }

            if (!await _companyRepository.CompanyExistAsync(request.CompanyId))
            {
                return response.SetError(404, "We could not find your company");
            }

            if (!await _employeeRepository.EmployeeExistAsync(request.EmployeeId))
            {
                return
                    response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            var workDays = await _workDayRepository.GetWorkDaysAsync(request.EmployeeId, request.ResourceParameters);

            response.Value = _mapper.Map<PagedList<WorkDayResponse>>(workDays);

            return response;
        }
    }
}
