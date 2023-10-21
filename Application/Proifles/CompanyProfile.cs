using System;
using AutoMapper;
using Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Company.Response;

namespace Application.Proifles
{
    class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Domain.Entities.Company, CompanyResponse>();
        }
    }
}
