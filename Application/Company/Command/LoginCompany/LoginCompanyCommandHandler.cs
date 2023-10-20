using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Company.Command.LoginCompany
{
    public class LoginCompanyCommandHandler : IRequestHandler<LoginCompanyCommand, string?>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public LoginCompanyCommandHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<string?> Handle(LoginCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies
                .Where(c => c.Email == request.Email)
                .FirstOrDefaultAsync();

            if (company == null) return null;

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password , company.Password);

            if (!isCorrectPassword) return null;

            return "Logged";
        }
    }
}
