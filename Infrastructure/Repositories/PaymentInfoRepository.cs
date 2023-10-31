﻿using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.PropertyMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PaymentInfoRepository : BaseRepository<EmployeePaymentInfo>, IPaymentInfoRepository
    {

        private readonly IPropertyMappingService _propertyMappingService;

        public PaymentInfoRepository
            (HrMeContext context , IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<EmployeePaymentInfo?> GetPaymentInfo(Guid paymentInfoId, Guid employeeId)
        {
            return await GetByIdQuery(paymentInfoId)
                 .Where(p => p.EmployeeId == employeeId)
                 .FirstOrDefaultAsync();
        }

        public async Task<IPagedList<EmployeePaymentInfo>>
            GetPaymentInfos(Guid employeeId , ResourceParameters resourceParameters)
        {
            var query = Query;

            var mapping = 
                _propertyMappingService.GetPropertyMapping<EmployeePaymentInfo , PaymentInfoResponse>(); 

            if(!resourceParameters.OrderBy.IsNullOrEmpty())
            {
                query = IQueraybleExtensions.ApplySort(query, resourceParameters.OrderBy!, mapping);
            }

            return await PagedList<EmployeePaymentInfo>
                .CreateAsync(query
                .Where(p => p.EmployeeId == employeeId),
                resourceParameters.PageNumber, resourceParameters.PageSize);
        }

        public async Task InsertPaymentInfo(EmployeePaymentInfo paymentInfo)
        {
            await Insert(paymentInfo);
        }
    }
}
