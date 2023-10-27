using Application.CQRS.Employee.Response;
using AutoMapper;
using Azure;
using Domain.Abstractions;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployees
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Response<IEnumerable<EmployeeResponse>>>
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

        public async Task<Response<IEnumerable<EmployeeResponse>>>
            Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            Response<IEnumerable<EmployeeResponse>> response = new();

            var comapnyExist = await _companyRepository.CompanyExistAsync(request.CompanyId);

            if (!comapnyExist)
            {
                return response.SetError(404, "We could not found your company in database");
            }

            var employeList = await _employeeRepository.GetEmployeesAsync(request.CompanyId);

            response.Value = _mapper.Map<IEnumerable<EmployeeResponse>>(employeList);

            return response;
        }
    }
}
