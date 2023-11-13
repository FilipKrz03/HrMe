﻿using Application.CQRS.PaymentInfo.Query.GetPaymentInfos;
using Application.Proifles;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Common;
using Infrastructure.PropertyMapping;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;

namespace Tests.PaymentInfos
{
    public class GetPaymentInfosQueryHandlerTests : CommandTestBase
    {
        private readonly IMapper _mapper;
        private readonly Mock<IPropertyMappingService> _propertyMappingServiceMock;

        public GetPaymentInfosQueryHandlerTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListConverter<,>));
                cfg.CreateMap<EmployeePaymentInfo, PaymentInfoResponse>();
            });

            _mapper = mapperConfiguration.CreateMapper();

            _propertyMappingServiceMock = new();
        }


        [Fact]
        public async Task Handler_Should_ReturnFailureResult_WhenOneOfOrderByFieldsDoNotExistInPaymentInfoFields()
        {
            var query = new GetPaymentInfosQuery
                (Guid.Empty, Guid.Empty, new Domain.Common.ResourceParameters());

            _propertyMappingServiceMock.Setup(x => x.PropertyMappingExist<EmployeePaymentInfo, PaymentInfoResponse>(
                It.IsAny<string>())).Returns(false);

            var handler = new GetPaymentInfosQueryHandler(_mapper, _companyRepositoryMock.Object,
                _employeeRepositoryMock.Object, _paymentInfoRepositoryMock.Object, _propertyMappingServiceMock.Object);

            var result = await handler.Handle(query, default);

            Assert.True(result.IsError);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Handler_Should_ReturnSuccesfulResult_WhenPropertyMappingExistAndEmployeeExist()
        {
            var query = new GetPaymentInfosQuery
               (Guid.Empty, Guid.Empty, new Domain.Common.ResourceParameters());

            _propertyMappingServiceMock.Setup(x => x.PropertyMappingExist<EmployeePaymentInfo, PaymentInfoResponse>(
                It.IsAny<string>())).Returns(true);

            _companyRepositoryMock.Setup(x => x.CompanyExistAsync(
                It.IsAny<Guid>())).ReturnsAsync(true);

            _employeeRepositoryMock.Setup(x => x.EmployeeExistsInCompanyAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);


            List<EmployeePaymentInfo> paymentInfos = new()
            {
                new EmployeePaymentInfo() ,
                new EmployeePaymentInfo(), 
            };

            _paymentInfoRepositoryMock.Setup(x => x.GetPaymentInfos(
                It.IsAny<Guid>(), It.IsAny<ResourceParameters>()))
                .ReturnsAsync(new PagedList<EmployeePaymentInfo>(paymentInfos, query.ResourceParameters.PageNumber,
                query.ResourceParameters.PageSize, 2));

            var handler = new GetPaymentInfosQueryHandler(_mapper, _companyRepositoryMock.Object,
                _employeeRepositoryMock.Object, _paymentInfoRepositoryMock.Object, _propertyMappingServiceMock.Object);

            var result = await handler.Handle(query, default);

            Assert.False(result.IsError);
            Assert.IsType<PagedList<PaymentInfoResponse>>(result.Value);
        }

    }
}
