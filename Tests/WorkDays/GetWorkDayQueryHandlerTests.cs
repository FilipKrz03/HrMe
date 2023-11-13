using Application.CQRS.WorkDay.Query.GetWorkDay;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Moq;
using Domain.Entities;
using Domain.Responses;
using Application.Common;

namespace Tests.WorkDays
{
    public class GetWorkDayQueryHandlerTests : CommandTestBase
    {

        private readonly Mock<IMapper> _mapperMock;

        public GetWorkDayQueryHandlerTests()
        {
            _mapperMock = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenEmployeeDoesNotExist()
        {
            var query = new GetWorkDayQuery(Guid.Empty, Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(false);

            var handler = new GetWorkDayQueryHandler(_mapperMock.Object, _workDayReposiotryMock.Object,
                _companyRepositoryMock.Object, _employeeRepositoryMock.Object);

            var result = await handler.Handle(query, default);

            Assert.True(result.IsError);
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenWorkDayDoesNotExist()
        {

            var query = new GetWorkDayQuery(Guid.Empty, Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _workDayReposiotryMock.Setup(x => x.GetWorkDayAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((EmployeeWorkDay)null!);

            var handler = new GetWorkDayQueryHandler(_mapperMock.Object, _workDayReposiotryMock.Object,
                _companyRepositoryMock.Object, _employeeRepositoryMock.Object);

            var result = await handler.Handle(query, default);

            Assert.True(result.IsError);

        }

        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenWorkDayExist()
        {
            var query = new GetWorkDayQuery(Guid.Empty, Guid.Empty, Guid.Empty);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _workDayReposiotryMock.Setup(x => x.GetWorkDayAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new EmployeeWorkDay());

            _mapperMock.Setup(x => x.Map<WorkDayResponse>(It.IsAny<EmployeeWorkDay>()))
                .Returns(new WorkDayResponse());

            var handler = new GetWorkDayQueryHandler(_mapperMock.Object, _workDayReposiotryMock.Object,
                _companyRepositoryMock.Object, _employeeRepositoryMock.Object);

            var result = await handler.Handle(query, default);

            Assert.False(result.IsError);
            Assert.IsType<WorkDayResponse>(result.Value);
        }
    }
}
