﻿using Domain.Entities;
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

        Task<bool> EmployeExistByEmaiInCompanylAsync(string email , Guid companyId);

        Task InsertEmployee(Employee employee);

        Task<Employee?> GetEmployeeAsync(Guid employeeId , Guid companyId);

        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId);
    }
}
