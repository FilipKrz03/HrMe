using Application.CQRS.Employee.Command.DeleteEmployee;
using System;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Domain.Entities;

namespace Tests.Employees
{
    public class DeleteEmployeeCommandHandlerTests : CommandTestBase
    {

        [Fact]
        public async Task Handler_ShouldReturnFailureWithCorrectStatusCode_WhenCompanyDoesNotExist()
        {
            var command = new DeleteEmployeeCommand(Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var handler = new DeleteEmployeeComandHandler(_employeeRepositoryMock.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task Handler_ShouldReturnFailureWithCorrectStatusCode_WhenEmployeeDoesNotExist()
        {
            var command = new DeleteEmployeeCommand(Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
            It.IsAny<Guid>()))
            .ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Entities.Employee)null!);

            var handler = new DeleteEmployeeComandHandler(_employeeRepositoryMock.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handler_ShouldCallDeleteToDepository_WhenEmployeeExist()
        {
            Guid employeeId = Guid.NewGuid();
            Guid companyId = Guid.NewGuid();

            var command = new DeleteEmployeeCommand(companyId , employeeId);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
            It.IsAny<Guid>()))
            .ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Employee()
                {
                    Id = employeeId,
                    CompanyId = companyId
                });

            var handler = new DeleteEmployeeComandHandler(_employeeRepositoryMock.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            _employeeRepositoryMock.Verify(x => x.DeleteEmployee(
                It.Is<Employee>(e => e.Id == employeeId && e.CompanyId == companyId)));
        }

        [Fact]
        public async Task Handler_ShouldReturnSuccesfullResult_WhenEmployeeIsDelated()
        {
            var command = new DeleteEmployeeCommand(Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
            It.IsAny<Guid>()))
            .ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Employee());

            var handler = new DeleteEmployeeComandHandler(_employeeRepositoryMock.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
        }
    }
}
