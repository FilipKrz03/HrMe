using Application.CQRS.WorkDay.Command.DeleteWorkDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Moq;
using Domain.Responses;
using Domain.Entities;

namespace Tests.WorkDays
{
    public class DeleteWorkDayCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenWorkDayDoNotExist()
        {
            var command = new DeleteWorkDayCommand(Guid.Empty , Guid.Empty , Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistsInCompanyAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            _workDayReposiotryMock.Setup(x => x.GetWorkDayAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((EmployeeWorkDay)null!);

            var handler = new DeleteWorkDayCommanHandler(_companyRepositoryMock.Object,
                _employeeRepositoryMock.Object, _workDayReposiotryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_CallDeleteToRepository_WhenWorkDayExist()
        {
            var command = new DeleteWorkDayCommand(Guid.Empty, Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistsInCompanyAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            _workDayReposiotryMock.Setup(x => x.GetWorkDayAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new EmployeeWorkDay());

            var handler = new DeleteWorkDayCommanHandler(_companyRepositoryMock.Object,
                _employeeRepositoryMock.Object, _workDayReposiotryMock.Object);

            var result = await handler.Handle(command, default);

            _workDayReposiotryMock.Verify(x => x.DeleteWorkDayAsync(
                It.IsAny<EmployeeWorkDay>()));
        }
        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenWorkDayDelated()
        {
            var command = new DeleteWorkDayCommand(Guid.Empty, Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistsInCompanyAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            _workDayReposiotryMock.Setup(x => x.GetWorkDayAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new EmployeeWorkDay());

            var handler = new DeleteWorkDayCommanHandler(_companyRepositoryMock.Object,
                _employeeRepositoryMock.Object, _workDayReposiotryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
        }
    }
}
