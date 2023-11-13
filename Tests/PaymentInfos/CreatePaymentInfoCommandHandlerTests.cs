using Application.CQRS.PaymentInfo.Command.CreatePaymentInfo;
using AutoMapper;
using Domain.Common.Enums;
using Domain.Entities;
using Domain.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.PaymentInfos
{
    public class CreatePaymentInfoCommandHandlerTests : CommandTestBase
    {

        private readonly Mock<IMapper> _mapperMock;

        public CreatePaymentInfoCommandHandlerTests()
        {
            _mapperMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResultWithCorrectStatusCode_WhenEmployeeDoNotExist()
        {
            var command = new CreatePaymentInfoCommand(Guid.Empty, Guid.Empty, 25, ContractType.ContractOfEmployment,
                DateTime.Now, DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((Employee)null!);


            var handler = new CreatePaymentInfoCommandHandler(_mapperMock.Object, _employeeRepositoryMock.Object,
                _companyRepositoryMock.Object, _paymentInfoRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenContractDateIsNotAvaliable()
        {
            var command = new CreatePaymentInfoCommand(Guid.Empty, Guid.Empty, 25, ContractType.ContractOfEmployment,
                DateTime.Now, DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Employee());

            _paymentInfoRepositoryMock.Setup(x => x.ContractDateIsNotAvaliableAsync(
               It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(true);

            var handler = new CreatePaymentInfoCommandHandler(_mapperMock.Object, _employeeRepositoryMock.Object,
                _companyRepositoryMock.Object, _paymentInfoRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenContractDataIsAvalaiableAndEmployeeExist()
        {
            var command = new CreatePaymentInfoCommand(Guid.Empty, Guid.Empty, 25, ContractType.ContractOfEmployment,
                           DateTime.Now, DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Employee());

            _paymentInfoRepositoryMock.Setup(x => x.ContractDateIsNotAvaliableAsync(
               It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<EmployeePaymentInfo>(It.IsAny<CreatePaymentInfoCommand>()))
          .Returns(new EmployeePaymentInfo());

            _mapperMock.Setup(x => x.Map<PaymentInfoResponse>(It.IsAny<EmployeePaymentInfo>()))
              .Returns(new PaymentInfoResponse());

            var handler = new CreatePaymentInfoCommandHandler(_mapperMock.Object, _employeeRepositoryMock.Object,
                _companyRepositoryMock.Object, _paymentInfoRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
            Assert.IsType<PaymentInfoResponse>(result.Value);
        }

        [Fact]
        public async Task Handler_Should_CallInsertToRepository_WhenContractDataIsAvalaiableAndEmployeeExist()
        {
            var command = new CreatePaymentInfoCommand(Guid.Empty, Guid.Empty, 25, ContractType.ContractOfEmployment,
                                       DateTime.Now, DateTime.Now);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.GetEmployeeAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new Employee());

            _paymentInfoRepositoryMock.Setup(x => x.ContractDateIsNotAvaliableAsync(
               It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<EmployeePaymentInfo>(It.IsAny<CreatePaymentInfoCommand>()))
                .Returns(new EmployeePaymentInfo());

            _mapperMock.Setup(x => x.Map<PaymentInfoResponse>(It.IsAny<EmployeePaymentInfo>()))
              .Returns(new PaymentInfoResponse());

            var handler = new CreatePaymentInfoCommandHandler(_mapperMock.Object, _employeeRepositoryMock.Object,
                _companyRepositoryMock.Object, _paymentInfoRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            _paymentInfoRepositoryMock.Verify(x => x.InsertPaymentInfo(
                It.IsAny<EmployeePaymentInfo>()));
        }
    }
}
