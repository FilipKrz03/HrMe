using Application.Common;
using Application.CQRS.Employee.Command.CreateEmployee;
using Domain.Responses;
using AutoMapper;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Employee.Command.PutEmployee;

namespace Application.Proifles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeCommand, Domain.Entities.Employee>();

            CreateMap<PutEmployeeComand, Domain.Entities.Employee>();

            CreateMap<Domain.Entities.Employee, EmployeeResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt =>
                opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Age, opt =>
                opt.MapFrom(src => DateTimeExtensions.CalculateAge(src.DateOfBirth)));

            
        }
    }
}
