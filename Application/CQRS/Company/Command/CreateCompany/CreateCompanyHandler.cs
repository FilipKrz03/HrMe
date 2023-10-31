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

namespace Application.CQRS.Company.Command.CreateCompany
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, Response<string>>
    {

        private readonly ICompanyRepository _comapnyRepository;

        public CreateCompanyHandler(ICompanyRepository companyRepostiory)
        {
            _comapnyRepository = companyRepostiory;
        }

        public async Task<Response<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            Response<string> response = new();

            var accountExist = await _comapnyRepository.CompanyExistByEmailAsync(request.Email);

            if (accountExist == false)
            {
                string hashedPwd = BCrypt.Net.BCrypt.HashPassword(request.Password);

                Domain.Entities.Company company = new()
                {
                    CompanyName = request.CompanyName,
                    Email = request.Email,
                    Password = hashedPwd
                };

                await _comapnyRepository.InsertCompany(company);

                response.Value = "Company created";

                return response;
            }

           return
                response.SetError(409, $"Account with email {request.Email} already exist");
        }
    }
}
