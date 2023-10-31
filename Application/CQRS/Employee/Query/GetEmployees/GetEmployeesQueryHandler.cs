using Domain.Responses;
using AutoMapper;
using Azure;
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

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<PagedList<EmployeeResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPropertyMappingService _propertyMappingService;

        public GetEmployeesQueryHandler(IMapper mapper, ICompanyRepository companyRepository, 
            IEmployeeRepository employeeRepository , IPropertyMappingService propertyMappingService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<Response<PagedList<EmployeeResponse>>>
            Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<EmployeeResponse>> response = new();

            if(!_propertyMappingService
                .PropertyMappingExist<Domain.Entities.Employee , EmployeeResponse>(request.ResourceParameters.OrderBy!))
            {
                return response.SetError(400 , $"One of orderByFields that you entered does not exist : " +
                    $"{request.ResourceParameters.OrderBy}");
            }

            var comapnyExist = await _companyRepository.CompanyExistAsync(request.CompanyId);

            if (!comapnyExist)
            {
                return response.SetError(404, "We could not found your company in database");
            }

            var employeList = await _employeeRepository.GetEmployeesAsync(request.CompanyId, request.ResourceParameters);

            response.Value = _mapper.Map<PagedList<EmployeeResponse>>(employeList);

            return response;
        }
    }
}
