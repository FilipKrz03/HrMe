﻿using Application.Common;
using Application.CQRS.WorkDay.Command.CreateWorkDay;
using Domain.Responses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.WorkDay.Command.PutWorkDay;

namespace Application.Proifles
{
    public class WorkDayProfile : Profile
    {
        public WorkDayProfile()
        {
            CreateMap<CreateWorkDayCommand, Domain.Entities.EmployeeWorkDay>()
                .ForMember(dest => dest.StartTimeInMinutesAfterMidnight,
                opt => opt.MapFrom(src => TimeOnlyExtensions.CalculateMinutesAfterMidnight(src.StartTime)))
                 .ForMember(dest => dest.EndTimeInMinutesAfterMidnight,
                opt => opt.MapFrom(src => TimeOnlyExtensions.CalculateMinutesAfterMidnight(src.EndTime)));

            CreateMap<PutWorkDayCommand, Domain.Entities.EmployeeWorkDay>()
                .ForMember(dest => dest.StartTimeInMinutesAfterMidnight,
                opt => opt.MapFrom(src => TimeOnlyExtensions.CalculateMinutesAfterMidnight(src.StartTime)))
                 .ForMember(dest => dest.EndTimeInMinutesAfterMidnight,
                opt => opt.MapFrom(src => TimeOnlyExtensions.CalculateMinutesAfterMidnight(src.EndTime)));

            CreateMap<Domain.Entities.EmployeeWorkDay, WorkDayResponse>()
                .ForMember(dest => dest.StartTime,
                opt => opt.MapFrom
                (src => TimeOnlyExtensions.CalculateTimeFromMinutesAfterMidnight(src.StartTimeInMinutesAfterMidnight)))
                .ForMember(dest => dest.EndTime,
                opt => opt.MapFrom
                (src => TimeOnlyExtensions.CalculateTimeFromMinutesAfterMidnight(src.EndTimeInMinutesAfterMidnight)))
                .ForMember(dest => dest.WorkDayDate,
                opt => opt.MapFrom(src => DateTimeExtensions.ConvertDateTimeOffSetToDateOnly(src.WorkDayDate)));
        }
    }
}
