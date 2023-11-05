using Application.CQRS.MonthlyBonus.Command.CreateMonthlyBonus;
using AutoMapper;
using Domain.Entities;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Proifles
{
    public class MonthlyBonusProfile : Profile
    {
        public MonthlyBonusProfile()
        {
            CreateMap<CreateMonthlyBonusCommand, EmployeeMonthlyBonus>();
            CreateMap<EmployeeMonthlyBonus , MonthlyBonusResponse>();
        }
    }
}
