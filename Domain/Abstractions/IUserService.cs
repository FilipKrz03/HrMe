using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IUserService
    {
        public Guid GetCompanyId();

        public Guid GetEmployeeId();
    }
}
