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
        protected Mock<ICompanyRepository> _companyRepositoryMock { get; }
        protected Mock<IEmployeeRepository> _employeeRepositoryMock {  get; }
        protected Mock<IWorkDayReposiotry> _workDayReposiotryMock { get; }
        protected Mock<IPaymentInfoRepository> _paymentInfoRepositoryMock {  get; }
        protected Mock<IMonthlyBonusRepository> _monthlyBonusRepositoryMock {  get; }

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
