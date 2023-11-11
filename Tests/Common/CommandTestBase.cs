using Domain.Abstractions;
using Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Common
{
    public class CommandTestBase : IDisposable
    {

        public Mock<ICompanyRepository> _companyRepositoryMock { get; }
        public Mock<IEmployeeRepository> _employeeRepositoryMock {  get; }
        public Mock<IWorkDayReposiotry> _workDayReposiotryMock { get; }
        public Mock<IPaymentInfoRepository> _paymentInfoRepositoryMock {  get; }
        public Mock<IMonthlyBonusRepository> _monthlyBonusRepositoryMock {  get; }

        public CommandTestBase()
        {
            _companyRepositoryMock = new();
            _employeeRepositoryMock = new();
            _workDayReposiotryMock = new();
            _paymentInfoRepositoryMock = new();
            _monthlyBonusRepositoryMock = new();
        }

        public void Dispose()
        {
            // Curently no needed
        }
    }
}
