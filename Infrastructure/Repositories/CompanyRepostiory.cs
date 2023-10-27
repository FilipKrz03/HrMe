using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CompanyRepostiory : BaseRepository<Company>, ICompanyRepository
    {

        public CompanyRepostiory(HrMeContext context):base(context) { }

        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            return await EntityExistAsync(companyId);
        }

        public async Task<bool> CompanyExistByEmailAsync(string companyEmail)
        {
            return await Query
                .AnyAsync(c => c.Email == companyEmail);
        }

        public Task<Company?> GetCompanyByEmailAsync(string companyEmail)
        {
            return Query
                .Where(c => c.Email == companyEmail)
                .FirstOrDefaultAsync();
        }

        public async Task InsertCompany(Company company)
        {
            await Insert(company);
        }
    }
}
