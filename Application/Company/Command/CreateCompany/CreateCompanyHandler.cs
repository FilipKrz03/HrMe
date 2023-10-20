using Application.Company.Response;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Company.Command.CreateCompany
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyResponse?>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public CreateCompanyHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CompanyResponse?> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var accountExist =
                await _context.Companies
                .AnyAsync(company => company.Email == request.Email);

            if (accountExist == false)
            {
                string hashedPwd = BCrypt.Net.BCrypt.HashPassword(request.Password);

                Domain.Entities.Company company = new()
                {
                    CompanyName = request.CompanyName,
                    Email = request.Email,
                    Password = hashedPwd
                };
                _context.Companies.Add(company);

                await _context.SaveChangesAsync(cancellationToken);

                var companyResponse = _mapper.Map<CompanyResponse>(company);
                return companyResponse;
            }
            return null;
        }
    }
}
