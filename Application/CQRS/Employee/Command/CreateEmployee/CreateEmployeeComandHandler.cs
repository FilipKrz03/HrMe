using Domain.Responses;
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
        private readonly IMailSendingService _mailSendingService;
        public CreateEmployeeComandHandler(IMapper mapper, ICompanyRepository companyRepository,
            IEmployeeRepository employeeRepository, IMailSendingService mailSendingService)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _mailSendingService = mailSendingService;
        }
        public async Task<Response<EmployeeResponse>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            Response<EmployeeResponse> response = new();

            if (!await _companyRepository.CompanyExistAsync(request.CompanyGuid))
            {
                return response.SetError(500, "We occured some unexpected error");
            }

            if (await _employeeRepository
                .EmployeExistWithEmailInCompanyAsync(request.Email, request.CompanyGuid))
            {
                return response.SetError(409,
                    $"The employe with email {request.Email} already exist in your company");
            }

            Domain.Entities.Employee employee
                = _mapper.Map<Domain.Entities.Employee>(request);

            employee.CompanyId = request.CompanyGuid;

            string password = string.Empty;

            if (request.Password == null)
            {
                string allowed = "ABCDEFGHIJKLMONOPQRSTUVWXYZabcdefghijklmonopqrstuvwxyz0123456789";
                char[] randomChars = new char[8];
                Random random = new Random();

                for (int i = 0; i < 8; i++)
                {
                    randomChars[i] = allowed[random.Next(0, allowed.Length)];
                }

                password = new string(randomChars);
            }
            else
            {
                password = request.Password;
            }

            string hashedPwd = BCrypt.Net.BCrypt.HashPassword(password);

            _mailSendingService.SendEmail
                (request.Email, "New account in HrMe application ! " ,
                "Your employee account has been created " +
                $"Your password : {password}" +
                $" Have a nice day ! ");

            employee.Password = hashedPwd;

            await _employeeRepository.InsertEmployee(employee);

            response.Value = _mapper.Map<EmployeeResponse>(employee);

            return response;
        }
    }
}
