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

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<PagedList<EmployeeResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;

        public GetEmployeesQueryHandler(IMapper mapper, ICompanyRepository companyRepository, 
            IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Response<PagedList<EmployeeResponse>>>
            Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            Response<PagedList<EmployeeResponse>> response = new();

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
