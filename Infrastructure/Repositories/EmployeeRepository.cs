using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(HrMeContext context) : base(context) { }

        public async Task<bool> EmployeeExistAsync(Guid companyId)
        {
            return await EntityExistAsync(companyId);
        }
    }
}
