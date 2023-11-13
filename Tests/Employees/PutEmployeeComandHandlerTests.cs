using Application.CQRS.Employee.Command.PutEmployee;
using AutoMapper;
using Domain.Entities;
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
    public class PutEmployeeComandHandlerTests : CommandTestBase
    {

        private readonly IMapper _mapper;

        public PutEmployeeComandHandlerTests()
        {
            var mapperConfiugration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PutEmployeeComand, Employee>();
                cfg.CreateMap<Employee, EmployeeResponse>();
            });

            _mapper = mapperConfiugration.CreateMapper();
        }

        [Fact]
        public async Task
            Handle_Should_ReturnFailureWithCorrectStatusCode_WhenEmployeeToPutExistAndOtherEmployeeWithSameEmailExist()
        {
            var command = new PutEmployeeComand
                (Guid.Empty, Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmial.com", DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Employee());

            _employeeRepositoryMock.Setup(x => x.OtherEmployeeExistWithSameMail(
              It.IsAny<string>(),
              It.IsAny<Guid>(),
              It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var handler = new PutEmployeeComandHandler
                (_employeeRepositoryMock.Object, _companyRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task
            Handle_Should_ReturnSuccessfulResult_WhenEmployeeToPutExistAndOtherEmployeeDoesNotHaveSameEmail()
        {
            var command = new PutEmployeeComand
                (Guid.Empty, Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmial.com", DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Employee());

            _employeeRepositoryMock.Setup(x => x.OtherEmployeeExistWithSameMail(
              It.IsAny<string>(),
              It.IsAny<Guid>(),
              It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var handler = new PutEmployeeComandHandler
                (_employeeRepositoryMock.Object, _companyRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
        }

        [Fact]
        public async Task
            Handler_Should_ReturnFailureResultWithProperStatusCode_WhenEmployeeToPutDoNotExistAndOtherEmployeeWithSameMailExist()
        {
            var command = new PutEmployeeComand
                (Guid.Empty, Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmial.com", DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Employee)null!);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
              It.IsAny<string>(),
              It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var handler = new PutEmployeeComandHandler
                (_employeeRepositoryMock.Object, _companyRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(409, result.StatusCode);
        }


        [Fact]
        public async Task
            Handler_Should_RetunSuccesfulResultAndCreatedEmployee_WhenEmployeeToPutNotExistAndOtherEmployeeWithSameEmailDoNotExist()
        {
            var command = new PutEmployeeComand
                (Guid.Empty, Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmial.com", DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Employee)null!);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
              It.IsAny<string>(),
              It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var handler = new PutEmployeeComandHandler
                (_employeeRepositoryMock.Object, _companyRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
            Assert.IsType<EmployeeResponse>(result.Value);
        }

        [Fact]
        public async Task
           Handler_Should_CallInsertToRepository_WhenEmployeeToPutNotExistAndOtherEmployeeWithSameEmailDoNotExist()
        {
            var command = new PutEmployeeComand
                (Guid.Empty, Guid.Empty, "Jan", "Kowalski", "Developer", "email@gmial.com", DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Employee)null!);

            _employeeRepositoryMock.Setup(x => x.EmployeExistWithEmailInCompanyAsync(
              It.IsAny<string>(),
              It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var handler = new PutEmployeeComandHandler
                (_employeeRepositoryMock.Object, _companyRepositoryMock.Object, _mapper);

            var result = await handler.Handle(command, default);

            _employeeRepositoryMock.Verify(x => x.InsertEmployee(
                It.Is<Employee>(e => e.Id == command.EmployeeId && e.CompanyId == command.CompanyId)));
        }

    }
}
