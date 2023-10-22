using Application.CQRS.Employee.Command.CreateEmployee;
using Application.CQRS.Employee.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Proifles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeCommand, Domain.Entities.Employee>();
            CreateMap<CreateEmployeeCommand, EmployeeResponse>()
                .ForMember(dest => dest.FullName, opt =>
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position));
        }
    }
}
