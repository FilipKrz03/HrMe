using Application.CQRS.Employee.Response;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Employee.Command.CreateEmployee
{
    public class CreateEmployeeComandHandler : IRequestHandler<CreateEmployeeCommand, Response<EmployeeResponse>>
    {
 
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        public CreateEmployeeComandHandler(IMapper mapper, ICompanyRepository companyRepository, 
            IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
        }
        public async Task<Response<EmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse> response = new();

            var employeeCompany = await _companyRepository.CompanyExistAsync(request.CompanyGuid);

            if (employeeCompany == false)
            {
                return response.SetError(500, "We occured some unexpected error");
            }

            var employeeExist = await _employeeRepository.
                EmployeExistByEmaiInCompanylAsync(request.Email , request.CompanyGuid);

            if (employeeExist != false)
            {
                return response.SetError(409,
                    $"The employe with email {request.Email} already exist in your company");
            }

            Domain.Entities.Employee employee
                = _mapper.Map<Domain.Entities.Employee>(request);

            employee.CompanyId = request.CompanyGuid;

            await _employeeRepository.InsertEmployee(employee);

            response.Value = _mapper.Map<EmployeeResponse>(employee);

            return response;
        }
    }
}
