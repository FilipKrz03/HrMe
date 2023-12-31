﻿using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<bool> EmployeeExistAsync(Guid employeeId);

        Task<bool> EmployeeExistsInCompanyAsync(Guid employeeId, Guid companyId);

        Task<bool> EmployeExistWithEmailInCompanyAsync(string email, Guid companyId);

        Task<Employee?> GetEmployeeByEmial(string email);

        Task InsertEmployee(Employee employee);

        Task<Employee?> GetEmployeeAsync(Guid employeeId, Guid companyId);

        Task<IPagedList<Employee>> GetEmployeesAsync(Guid companyId, ResourceParameters resourceParameters);

        Task DeleteEmployee(Employee employee);

        Task SaveChangesAsync();

        Task<bool> OtherEmployeeExistWithSameMail(string mailToCheck, Guid companyId, Guid employeeId);

        Task<IPagedList<Employee>> GetEmployeesWithPaymentDataForMonth(Guid companyId, int year, int month,
           ResourceParameters resourceParameters);

        Task<Employee?> GetEmployeeWithPaymentDataForMonth(Guid companyId, Guid employeeId, int year, int month);
    }
}
