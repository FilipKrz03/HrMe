using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ICompanyRepository
    {
        Task<bool> CompanyExistAsync(Guid companyId);

        Task<bool> CompanyExistByEmailAsync(string companyEmail);

        Task InsertCompany(Company company);

        Task<Company?> GetCompanyByEmailAsync(string companyEmail);    
    }
}
