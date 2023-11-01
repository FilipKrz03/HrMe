using Domain.Abstractions;
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
            (HrMeContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<EmployeePaymentInfo?> GetPaymentInfo(Guid paymentInfoId, Guid employeeId)
        {
            return await GetByIdQuery(paymentInfoId)
                 .Where(p => p.EmployeeId == employeeId)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeePaymentInfo>> GetAllPaymentInfos(Guid employeeId)
        {
            return await Query.Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IPagedList<EmployeePaymentInfo>>
            GetPaymentInfos(Guid employeeId, ResourceParameters resourceParameters)
        {
            var query = Query;

            var mapping =
                _propertyMappingService.GetPropertyMapping<EmployeePaymentInfo, PaymentInfoResponse>();

            if (!resourceParameters.SearchQuery.IsNullOrEmpty())
            {
                query = query.Where
                    (p => p.HourlyRateBrutto.ToString().Contains(resourceParameters.SearchQuery!)
                    || p.StartOfContractDate.ToString().Contains(resourceParameters.SearchQuery!)
                    || p.EndOfContractDate.ToString()!.Contains(resourceParameters.SearchQuery!)
                    || p.Id.ToString().Contains(resourceParameters.SearchQuery!)
                    );
            }

            if (!resourceParameters.OrderBy.IsNullOrEmpty())
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

        public async Task DeletePaymentInfo(EmployeePaymentInfo paymentInfo)
        {
            await DeleteEntity(paymentInfo);
        }

        public async Task<bool> ContractDateIsNotAvaliableAsync
            (DateTime start, DateTime? end)
        {
            return await Query
                .AnyAsync(c =>
                (start < c.EndOfContractDate && end >= c.StartOfContractDate)
                || (c.EndOfContractDate == null && start >= c.StartOfContractDate)
                || (end == null && start <= c.EndOfContractDate));

        }

        public async Task<bool> OtherContractIsPending(DateTime start, DateTime? end, Guid currentContractId)
        {
            return await Query
                      .AnyAsync(c =>
                      ((start < c.EndOfContractDate && end >= c.StartOfContractDate)
                      || (c.EndOfContractDate == null && start >= c.StartOfContractDate)
                      || (end == null && start <= c.EndOfContractDate))
                      && (c.Id != currentContractId)
                      );
        }

        async Task IPaymentInfoRepository.SaveChangesAsync()
        {
            await SaveChangesAsync();
        }

    }
}
