using Application.CQRS.Employee.Response;
using AutoMapper;
using Domain.Abstractions;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Query.GetEmployee
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Response<EmployeeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;

        public GetEmployeeQueryHandler(IMapper mapper , ICompanyRepository companyRepository ,
            IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<Response<EmployeeResponse>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse> response = new();

            var comapnyExist = await _companyRepository.CompanyExistAsync(request.CompanyId);

            if (!comapnyExist)
            {
                return response.SetError(404, "We could not found your company in database");
            }

            var employee = await _employeeRepository.GetEmployeeAsync(request.EmployeeId , request.CompanyId);

            if (employee == null)
            {
                return response.SetError(404, $"We could not find employee with id {request.EmployeeId}");
            }

            response.Value = _mapper.Map<EmployeeResponse>(employee);

            return response;
        }
    }
}
