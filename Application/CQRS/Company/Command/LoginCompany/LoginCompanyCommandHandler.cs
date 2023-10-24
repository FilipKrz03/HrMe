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
        private readonly HrMeContext _context;
        private readonly IJwtProvider _jwtProvider;

        public LoginCompanyCommandHandler(HrMeContext context, IMapper mapper,
            IJwtProvider jwtProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        }

        public async Task<Response<string>> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {

            Response<string> response = new();

            var company = await _context.Companies
                .Where(c => c.Email == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (company == null)
            {
                response.SetError(400,
                    $"Company with emai1 {request.Email} does not exist");
                return response;
            } 

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password, company.Password);

            if (!isCorrectPassword)
            {
                response.SetError(403, "Invalid password");
                return response;
            }

            string token = _jwtProvider.Generate(request.Email, company.Id);

            response.Value = token;

            return response;
        }
    }
}
