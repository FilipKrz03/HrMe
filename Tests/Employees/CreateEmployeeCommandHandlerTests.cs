using Application.CQRS.Employee.Command.CreateEmployee;
using AutoMapper;
using Domain.Abstractions;
using Domain.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.Employees
{
    public class CreateEmployeeCommandHandlerTests : CommandTestBase
    {

        private readonly IMapper _mapper;
        private readonly Mock<IMailSendingService> _mailSendingServiceMock;

        public CreateEmployeeCommandHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateEmployeeCommand, Domain.Entities.Employee>();
                cfg.CreateMap<Domain.Entities.Employee, EmployeeResponse>();
            });

            _mapper = mapperConfig.CreateMapper();

            _mailSendingServiceMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResultWithCorrectStatusCode_WhenCompanyDoesNotExist()
        {
            var command = new CreateEmployeeCommand
                (Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmail.com", DateTime.Now, "DummyPassword");

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(false);

            var handler = new CreateEmployeeComandHandler
                (_mapper, _companyRepositoryMock.Object, _employeeRepositoryMock.Object,
                _mailSendingServiceMock.Object);

            var result = await handler.Handle(command, default);

            Assert.Equal(500, result.StatusCode);
            Assert.True(result.IsError);
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmployeeWithSameEmailExist()
        {
            var command = new CreateEmployeeCommand
               (Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmail.com", DateTime.Now, "DummyPassword");

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
               It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(true);

            var handler = new CreateEmployeeComandHandler
                (_mapper, _companyRepositoryMock.Object, _employeeRepositoryMock.Object,
                _mailSendingServiceMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_GeneratePassword_WhenPasswordIsNotEntered()
        {
            var command = new CreateEmployeeCommand
              (Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmail.com", DateTime.Now , null);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
               It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var handler = new CreateEmployeeComandHandler
               (_mapper, _companyRepositoryMock.Object, _employeeRepositoryMock.Object,
               _mailSendingServiceMock.Object);

            var result = await handler.Handle(command, default);

            _employeeRepositoryMock.Verify(x => x.InsertEmployee(
                It.Is<Domain.Entities.Employee>(e => e.Password != null)));
        }

        [Fact]
        public async Task Handler_Should_CallInsertToRepository_WhenEmployeeIsCreated()
        {
            var command = new CreateEmployeeCommand
              (Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmail.com", DateTime.Now, "DummyPassword");

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
               It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var handler = new CreateEmployeeComandHandler
                (_mapper, _companyRepositoryMock.Object, _employeeRepositoryMock.Object,
                _mailSendingServiceMock.Object);

            var result = await handler.Handle(command, default);

            _employeeRepositoryMock.Verify(x => x.InsertEmployee(
                It.Is<Domain.Entities.Employee>(e => e.Email == command.Email)));
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenEmployeeIsCreated()
        {
            var command = new CreateEmployeeCommand
              (Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmail.com", DateTime.Now, "DummyPassword");

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
               It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(false);

            var handler = new CreateEmployeeComandHandler
                (_mapper, _companyRepositoryMock.Object, _employeeRepositoryMock.Object,
                _mailSendingServiceMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
            Assert.IsType<EmployeeResponse>(result.Value);
        }
    }
}
