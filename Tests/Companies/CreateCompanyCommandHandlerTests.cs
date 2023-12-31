﻿using Application.CQRS.Company.Command.CreateCompany;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Responses;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.Companies
{
    public class CreateCompanyCommandHandlerTests : CommandTestBase
    {
        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenCompanyEmailAlreadyExist()
        {
            var command = new CreateCompanyCommand()
            {
                CompanyName = "Dommy",
                Email = "existingmail@gmail.com",
                Password = "Dummy password",
            };

            _companyRepositoryMock.Setup(x => x.CompanyExistByEmailAsync(
                It.IsAny<string>())).ReturnsAsync(true);

            var handler = new CreateCompanyHandler(_companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.True(result.IsError);
        }

        [Fact]
        public async Task Handle_Should_ReturnSuccesfulResult_WhenEmailIsUnic()
        {
            var command = new CreateCompanyCommand()
            {
                CompanyName = "Dommy",
                Email = "notexistingmail@gmail.com",
                Password = "Dummy password",
            };

            _companyRepositoryMock.Setup(x => x.CompanyExistByEmailAsync(
               It.IsAny<string>())).ReturnsAsync(false);

            var handler = new CreateCompanyHandler(_companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            Assert.False(result.IsError);
            Assert.IsType<string>(result.Value);
        }

        [Fact]
        public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUniqe()
        {
            var command = new CreateCompanyCommand()
            {
                CompanyName = "Dommy",
                Email = "notexistingmail@gmail.com",
                Password = "Dummy password",
            };

            _companyRepositoryMock.Setup(x => x.CompanyExistByEmailAsync(
               It.IsAny<string>())).ReturnsAsync(false);

            var handler = new CreateCompanyHandler(_companyRepositoryMock.Object);

            var result = await handler.Handle(command, default);

            _companyRepositoryMock.Verify(x => x.InsertCompany(
               It.Is<Company>(c => c.Email == command.Email)) , Times.Once);
        }
    }
}
