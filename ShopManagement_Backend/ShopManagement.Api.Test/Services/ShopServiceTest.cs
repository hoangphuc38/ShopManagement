using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Api.Test.Services
{
    public class ShopServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IShopRepository> _shopRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IShopDetailRepository> _shopDetailRepoMock;
        private readonly Mock<ILogger<ShopService>> _loggerMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<ResourceManager> _resourceMock;
        private readonly ShopService _shopServiceMock;

        public ShopServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _shopRepoMock = new Mock<IShopRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _shopDetailRepoMock = new Mock<IShopDetailRepository>();
            _loggerMock = new Mock<ILogger<ShopService>>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _resourceMock = new Mock<ResourceManager>();

            _shopServiceMock = new ShopService(
                _mapperMock.Object,
                _shopRepoMock.Object,
                _userRepoMock.Object,
                _shopDetailRepoMock.Object,
                _loggerMock.Object,
                _memoryCacheServiceMock.Object);
        }

        [Fact]
        public async Task GetShopWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            int totalRecords = 2;

            _shopRepoMock.Setup(r => r.GetShopsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(shopObject.shopList);
            _mapperMock.Setup(m => m.Map<List<ShopResponse>>(It.IsAny<List<Shop>>()))
                .Returns(shopObject.shopListMap);

            //Act
            var result = await _shopServiceMock.GetShopsWithPagination(shopObject.paginationRequest);

            int totalPages = 2 / shopObject.paginationRequest.PageSize + 1;

            var res = new PaginationResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalOfNumberRecord = 2,
                TotalOfPages = 1,
                Results = shopObject.shopListMap,
            };

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(res.PageNumber, response.PageNumber);
            Assert.Equal(res.PageSize, response.PageSize);
            Assert.Equal(res.TotalOfNumberRecord, response.TotalOfNumberRecord);
            Assert.Equal(res.TotalOfPages, totalPages);
        }

        [Fact]
        public async Task GetShopWithPagination_TotalPageReturn0_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            int totalRecords = 0;

            _shopRepoMock.Setup(r => r.GetShopsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(new List<Shop>());
            _mapperMock.Setup(m => m.Map<List<ShopResponse>>(It.IsAny<List<Shop>>()))
                .Returns(new List<ShopResponse>());

            //Act
            var result = await _shopServiceMock.GetShopsWithPagination(shopObject.paginationRequest);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(0, response.TotalOfNumberRecord);
            Assert.Equal(1, response.TotalOfPages);
        }

        [Fact]
        public async Task GetShopWithPagination_InvalidPageIndex_ShouldReturnStatusCode400()
        {
            //Arrange 
            var request = new ShopPaginationRequest
            {
                PageIndex = 0,
                PageSize = 10,
            };

            //Act
            var result = await _shopServiceMock.GetShopsWithPagination(request);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task GetShopWithPagination_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange 
            var shopObject = new ShopTest();

            _shopRepoMock.Setup(r => r.GetShopsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                .Throws(new Exception("failed"));

            //Act
            var result = await _shopServiceMock.GetShopsWithPagination(shopObject.paginationRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task GetShopOfUser_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            var userObject = new UserTest();
            int totalRecords = 2;

            _shopRepoMock.Setup(r => r.GetShopByUserID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(shopObject.shopList);
            _userRepoMock.Setup(r => r.GetUserByShopID(It.IsAny<int>()))
                .ReturnsAsync(userObject.user);
            _mapperMock.Setup(m => m.Map<List<ShopResponse>>(It.IsAny<List<Shop>>()))
                .Returns(shopObject.shopListMap);

            //Act
            var result = await _shopServiceMock.GetShopOfUser(1, shopObject.paginationRequest);

            int totalPages = 2 / shopObject.paginationRequest.PageSize + 1;

            var res = new PaginationResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalOfNumberRecord = 2,
                TotalOfPages = 1,
                Results = shopObject.shopListMap,
            };

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(res.PageNumber, response.PageNumber);
            Assert.Equal(res.PageSize, response.PageSize);
            Assert.Equal(res.TotalOfNumberRecord, response.TotalOfNumberRecord);
            Assert.Equal(res.TotalOfPages, totalPages);
        }

        [Fact]
        public async Task GetShopOfUser_TotalPageReturn0_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            int totalRecords = 0;

            _shopRepoMock.Setup(r => r.GetShopByUserID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(new List<Shop>());
            _mapperMock.Setup(m => m.Map<List<ShopResponse>>(It.IsAny<List<Shop>>()))
                .Returns(new List<ShopResponse>());

            //Act
            var result = await _shopServiceMock.GetShopOfUser(1, shopObject.paginationRequest);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(0, response.TotalOfNumberRecord);
            Assert.Equal(1, response.TotalOfPages);
        }

        [Fact]
        public async Task GetShopOfUser_InvalidPageIndex_ShouldReturnStatusCode400()
        {
            //Arrange 
            var request = new ShopPaginationRequest
            {
                PageIndex = 0,
                PageSize = 10,
            };

            //Act
            var result = await _shopServiceMock.GetShopOfUser(1, request);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task GetShopOfUser_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange 
            var shopObject = new ShopTest();

            _shopRepoMock.Setup(r => r.GetShopByUserID(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _shopServiceMock.GetShopOfUser(1, shopObject.paginationRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }
    }
}
