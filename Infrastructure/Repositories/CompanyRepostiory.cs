using Domain.Abstractions;
using Domain.Entities;
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
    }
}
