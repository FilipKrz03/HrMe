using Application.CQRS.Company.Response;
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

namespace Application.CQRS.Company.Command.CreateCompany
{
    public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, Response<string?>>
    {
        private readonly HrMeContext _context;
        private readonly IMapper _mapper;

        public CreateCompanyHandler(HrMeContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<string?>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            Response<string?> response = new();

            var accountExist =
                await _context.Companies
                .AnyAsync(company => company.Email == request.Email, cancellationToken);

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

                response.Value = "Company created";

                return response;
            }

            response.SetError(409, $"Account with email {request.Email} already exist");
            return response;
        }
    }
}
