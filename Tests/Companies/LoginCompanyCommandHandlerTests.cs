using Application.CQRS.Company.Command.LoginCompany;
using Moq;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Domain.Entities;

namespace Tests.Companies
{
    public class LoginCompanyCommandHandlerTests : CommandTestBase
    {
        private readonly Mock<IJwtProvider> _jwtProvider;

        public LoginCompanyCommandHandlerTests()
        {
            _jwtProvider = new();
        }

        [Fact]
        public async Task Handler_Should_ReturnFailureResultWithCorrectStatusCode_WhenCompanyDoesNotExist()
        {
            var command = new LoginCompanyCommand()
            {
                Email = "testemail@gmail.com",
                Password = "DummyPassword"
            };

            _companyRepositoryMock.Setup(x => x.GetCompanyByEmailAsync(
                It.IsAny<string>())).ReturnsAsync((Company)null!);

            var handler = new LoginCompanyCommandHandler
                (_jwtProvider.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(400, result.StatusCode);
        }


        [Fact]
        public async Task Handler_Should_ReturnFailureResultWithCorrectStatusCode_WhenPasswordsDoNotMatch()
        {
            var command = new LoginCompanyCommand()
            {
                Email = "testemail@gmail.com",
                Password = "DummyBadPassword"
            };

            _companyRepositoryMock.Setup(x => x.GetCompanyByEmailAsync(
                It.IsAny<string>())).ReturnsAsync(new Company
                {
                    Password = BCrypt.Net.BCrypt.HashPassword("DummyGoodPassword")
                });


            var handler = new LoginCompanyCommandHandler
                (_jwtProvider.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenPasswordsDoNotMatch()
        {
            var command = new LoginCompanyCommand()
            {
                Email = "testemail@gmail.com",
                Password = "DummyGoodPassword"
            };

            _companyRepositoryMock.Setup(x => x.GetCompanyByEmailAsync(
                It.IsAny<string>())).ReturnsAsync(new Company
                {
                    Password = BCrypt.Net.BCrypt.HashPassword("DummyGoodPassword")
                });

            _jwtProvider.Setup(x => x.Generate(
                It.IsAny<string>(), It.IsAny<Guid>(), true, null))
                .Returns("DummyAccesToken");

            var handler = new LoginCompanyCommandHandler
                (_jwtProvider.Object, _companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
            Assert.IsType<string>(result.Value);
        }
    }
}
