using Application.CQRS.PaymentInfo.Command.CreatePaymentInfo;
using Application.CQRS.PaymentInfo.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Proifles
{
    public class PaymentInfoProfile : Profile
    {
        public PaymentInfoProfile()
        {
            CreateMap<CreatePaymentInfoCommand, Domain.Entities.EmployeePaymentInfo>();
            CreateMap<Domain.Entities.EmployeePaymentInfo, PaymentInfoResponse>();
        }
    }
}
