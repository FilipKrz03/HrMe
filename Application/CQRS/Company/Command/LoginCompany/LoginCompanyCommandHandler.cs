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

namespace Application.CQRS.Company.Command.LoginCompany
{
    public class LoginCompanyCommandHandler : IRequestHandler<LoginCompanyCommand, Response<string>>
    {

        private readonly IJwtProvider _jwtProvider;
        private readonly ICompanyRepository _companyRepository;

        public LoginCompanyCommandHandler(IJwtProvider jwtProvider, ICompanyRepository companyRepository)
        {
            _jwtProvider = jwtProvider;
            _companyRepository = companyRepository;
        }

        public async Task<Response<string>> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {

            Response<string> response = new();

            var company = await _companyRepository.GetCompanyByEmailAsync(request.Email);

            if (company == null)
            {
                return response.SetError(400,
                      $"Company with emai1 {request.Email} does not exist");
            }

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password, company.Password);

            if (!isCorrectPassword)
            {
                return response.SetError(403, "Invalid password");
            }

            string token = _jwtProvider.Generate(request.Email, company.Id , true , null);

            response.Value = token;

            return response;
        }
    }
}
