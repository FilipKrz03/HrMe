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
    public class LoginCompanyCommandHandler : IRequestHandler<LoginCompanyCommand, string?>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public LoginCompanyCommandHandler(HrMeContext context, IMapper mapper,
            IJwtProvider jwtProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        }

        public async Task<string?> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies
                .Where(c => c.Email == request.Email)
                .FirstOrDefaultAsync();

            if (company == null) return null;

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password, company.Password);

            if (!isCorrectPassword) return null;

            string token = _jwtProvider.Generate(request.Email);

            return token;
        }
    }
}
