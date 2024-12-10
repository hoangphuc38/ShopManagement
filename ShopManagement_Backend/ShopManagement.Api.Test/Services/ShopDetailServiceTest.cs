using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Resources;

namespace ShopManagement.Api.Test.Services
{
    public class ShopDetailServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IShopDetailRepository> _shopDetailRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IShopRepository> _shopRepoMock;
        private readonly Mock<ILogger<ShopDetailService>> _loggerMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<ResourceManager> _resourceMock;
        private readonly ShopDetailService _shopDetailServiceMock;

        public ShopDetailServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _shopDetailRepoMock = new Mock<IShopDetailRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _shopRepoMock = new Mock<IShopRepository>();
            _loggerMock = new Mock<ILogger<ShopDetailService>>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _resourceMock = new Mock<ResourceManager>();

            _shopDetailServiceMock = new ShopDetailService(
                _mapperMock.Object,
                _shopDetailRepoMock.Object,
                _productRepoMock.Object,
                _shopRepoMock.Object,
                _loggerMock.Object,
                _memoryCacheServiceMock.Object);
        }

        [Fact]
        public async Task GetShopDetailWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            int totalRecords = 2;

            _shopDetailRepoMock.Setup(r => r.GetShopDetailWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(shopDetailObject.shopDetailList);

            _mapperMock.Setup(m => m.Map<List<ShopDetailResponse>>(It.IsAny<List<ShopDetail>>()))
                .Returns(shopDetailObject.shopDetailListMap);

            //Act
            var result = await _shopDetailServiceMock.GetShopDetailWithPagination(1, shopDetailObject.paginationRequest);

            int totalPages = 2 / shopDetailObject.paginationRequest.PageSize + 1;

            var res = new PaginationResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalOfNumberRecord = 2,
                TotalOfPages = 1,
                Results = shopDetailObject.shopDetailListMap,
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
        public async Task GetShopDetailWithPagination_TotalPageReturn0_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            int totalRecords = 0;

            _shopDetailRepoMock.Setup(r => r.GetShopDetailWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(new List<ShopDetail>());

            _mapperMock.Setup(m => m.Map<List<ShopDetailResponse>>(It.IsAny<List<ShopDetail>>()))
                .Returns(new List<ShopDetailResponse>());

            //Act
            var result = await _shopDetailServiceMock.GetShopDetailWithPagination(1, shopDetailObject.paginationRequest);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(0, response.TotalOfNumberRecord);
            Assert.Equal(1, response.TotalOfPages);
            Assert.Equal(new List<ShopDetailResponse>(), response.Results);
        }

        [Fact]
        public async Task GetShopDetailWithPagination_InvalidPageIndex_ShouldReturnStatusCode400()
        {
            //Arrange 
            var request = new ShopDetailPaginationRequest
            {
                PageIndex = 0,
                PageSize = 10,
            };

            //Act
            var result = await _shopDetailServiceMock.GetShopDetailWithPagination(1, request);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task GetShopDetailWithPagination_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();

            _shopDetailRepoMock.Setup(r => r.GetShopDetailWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _shopDetailServiceMock.GetShopDetailWithPagination(1, shopDetailObject.paginationRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task UpdateDetail_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shop);
            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetail);
            _shopDetailRepoMock.Setup(r => r.UpdateAsync(shopDetailObject.shopDetail));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _shopDetailServiceMock.UpdateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task UpdateDetail_NotFoundProduct_ShouldReturnStatusCode404()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.productNull);

            //Act
            var result = await _shopDetailServiceMock.UpdateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task UpdateDetail_NotFoundShop_ShouldReturnStatusCode404()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shopNull);

            //Act
            var result = await _shopDetailServiceMock.UpdateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task UpdateDetail_NotFoundDetail_ShouldReturnStatusCode404()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shop);
            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetailNull);

            //Act
            var result = await _shopDetailServiceMock.UpdateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task UpdateDetail_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _shopDetailServiceMock.UpdateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]  
        public async Task DeleteDetail_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();

            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetail);
            _shopDetailRepoMock.Setup(r => r.DeleteAsync(shopDetailObject.shopDetail));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _shopDetailServiceMock.DeleteDetail(1, 1);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task DeleteDetail_NotFoundDetail_ShouldReturnStatusCode404()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();

            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetailNull);

            //Act
            var result = await _shopDetailServiceMock.DeleteDetail(1, 1);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task DeleteDetail_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .Throws(new Exception("failed"));

            //Act
            var result = await _shopDetailServiceMock.DeleteDetail(1, 1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task CreateDetail_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shop);
            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetailNull);
            _mapperMock.Setup(m => m.Map<ShopDetail>(It.IsAny<ShopDetailRequest>()))
                .Returns(shopDetailObject.shopDetail);
            _shopDetailRepoMock.Setup(r => r.AddAsync(It.IsAny<ShopDetail>()));

            //Act
            var result = await _shopDetailServiceMock.CreateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task CreateDetailWhenDetailExist_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shop);
            _shopDetailRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetail);
            _shopDetailRepoMock.Setup(r => r.UpdateAsync(shopDetailObject.shopDetail));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _shopDetailServiceMock.CreateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task CreateDetail_NotFoundProduct_ShouldReturnStatusCode404()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.productNull);

            //Act
            var result = await _shopDetailServiceMock.CreateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task CreateDetail_NotFoundShop_ShouldReturnStatusCode404()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var productObject = new ProductTest();
            var shopObject = new ShopTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _shopRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shopNull);

            //Act
            var result = await _shopDetailServiceMock.CreateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task CreateDetail_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _shopDetailServiceMock.CreateDetail(shopDetailObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }
    }
}
