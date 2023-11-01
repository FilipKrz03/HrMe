using Application.CQRS.PaymentInfo.Command.CreatePaymentInfo;
using Domain.Responses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.PaymentInfo.Command.PutPaymentInfo;

namespace Application.Proifles
{
    public class PaymentInfoProfile : Profile
    {
        public PaymentInfoProfile()
        {
            CreateMap<CreatePaymentInfoCommand, Domain.Entities.EmployeePaymentInfo>();
            CreateMap<PutPaymentInfoCommand, Domain.Entities.EmployeePaymentInfo>();
            CreateMap<Domain.Entities.EmployeePaymentInfo, PaymentInfoResponse>();
        }
    }
}
