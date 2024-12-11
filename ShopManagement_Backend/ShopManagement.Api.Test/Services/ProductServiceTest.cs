using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Hubs;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Resources;

namespace ShopManagement.Api.Test.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IShopDetailRepository> _shopDetailRepoMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<INotificationRepository> _notiRepoMock;
        private readonly Mock<INotificationRecepientRepository> _notiRecepRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly Mock<IHubContext<StockHub>> _stockHubMock;
        private readonly Mock<ResourceManager> _resourceMock;
        private readonly ProductService _productServiceMock;

        public ProductServiceTest()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _shopDetailRepoMock = new Mock<IShopDetailRepository>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _notiRepoMock = new Mock<INotificationRepository>();
            _notiRecepRepoMock = new Mock<INotificationRecepientRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _stockHubMock = new Mock<IHubContext<StockHub>>();
            _resourceMock = new Mock<ResourceManager>();

            _productServiceMock = new ProductService(
                _mapperMock.Object,
                _productRepoMock.Object,
                _shopDetailRepoMock.Object,
                _notiRepoMock.Object,
                _notiRecepRepoMock.Object,
                _userRepoMock.Object,
                _loggerMock.Object,
                _memoryCacheServiceMock.Object,
                _stockHubMock.Object);
        }

        [Fact]
        public async Task GetProductWithPagination_OnSuccess_ShouldReturnData()
        {
            //Arrange
            var productObject = new ProductTest();
            int totalRecords = 2;

            _productRepoMock.Setup(repo => repo.GetProductsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(productObject.productsBeforeMap);

            _mapperMock.Setup(mapper => mapper.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
                .Returns(productObject.Products);

            //Act
            var result = await _productServiceMock.GetProductsWithPagination(productObject.productRequest);

            int totalPages = 2 / productObject.productRequest.PageSize + 1;

            var res = new PaginationResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalOfNumberRecord = 2,
                TotalOfPages = 1,
                Results = productObject.Products,
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
            Assert.Equal(res.Results, response.Results);
        }

        [Fact]
        public async Task GetProductWithPagination_TotalRecordsReturn0_ShouldReturnData()
        {
            //Arrange
            var productObject = new ProductTest();
            int totalRecords = 0;

            _productRepoMock.Setup(repo => repo.GetProductsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(new List<Product>());

            _mapperMock.Setup(mapper => mapper.Map<List<ProductResponse>>(It.IsAny<List<Product>>()))
                .Returns(new List<ProductResponse>());

            //Act
            var result = await _productServiceMock.GetProductsWithPagination(productObject.productRequest);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(0, response.TotalOfNumberRecord);
            Assert.Equal(1, response.TotalOfPages);
            Assert.Equal(new List<ProductResponse>(), response.Results);
        }

        [Fact]
        public async Task GetProductWithPagination_InvalidPageIndex_ShouldReturnStatusCode400()
        {
            //Arrange
            var request = new ProductPaginationRequest { PageIndex = 0, PageSize = 10 };

            //Act
            var result = await _productServiceMock.GetProductsWithPagination(request);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task GetProductWithPagination_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();

            _productRepoMock.Setup(repo => repo.GetProductsWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _productServiceMock.GetProductsWithPagination(productObject.productRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task GetDetailProductWithCache_OnSuccess_ShouldReturnData()
        {
            //Arrange 
            var productObject = new ProductTest();
            var response = new BaseResponse(productObject.Products);

            _memoryCacheServiceMock.Setup(c => c.GetCacheData(It.IsAny<string>()))
                .Returns(response);

            //Act
            var result = await _productServiceMock.GetDetailProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetDetailProductNoCache_OnSuccess_ShouldReturnData()
        {
            //Arrange 
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(productObject.product);
            _mapperMock.Setup(m => m.Map<ProductResponse>(It.IsAny<Product>()))
                .Returns(productObject.productMap);
            _memoryCacheServiceMock.Setup(c => c.SetCache(It.IsAny<string>(), It.IsAny<BaseResponse>()));
                
            //Act
            var result = await _productServiceMock.GetDetailProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetDetailProductNoCache_NotFoundProduct_ShouldReturnStatusCode404()
        {
            //Arrange 
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(productObject.productNull);

            //Act
            var result = await _productServiceMock.GetDetailProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetDetailProduct_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            _productRepoMock.Setup(repo => repo.GetProductById(It.IsAny<int>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _productServiceMock.GetDetailProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task UpdateProduct_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _productRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _productServiceMock.UpdateProduct(1, productObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task UpdateProduct_NotFoundProduct_ShouldReturnStatusCode404()
        {
            //Arrange
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.productNull);

            //Act
            var result = await _productServiceMock.UpdateProduct(1, productObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task UpdateProduct_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _productRepoMock.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _productServiceMock.UpdateProduct(1, productObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task DeleteProduct_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _productRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()));
            _shopDetailRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetailList);
            _shopDetailRepoMock.Setup(r => r.UpdateAsync(It.IsAny<ShopDetail>()));

            //Act
            var result = await _productServiceMock.DeleteProduct(1);

            productObject.product.IsDeleted = true;

            foreach (var detail in shopDetailObject.shopDetailList)
            {
                detail.IsDeleted = true;
            }

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.True(productObject.product.IsDeleted);
        }

        [Fact]
        public async Task DeleteProduct_NotFoundProduct_ShouldReturnStatusCode404()
        {
            //Arrange
            var productObject = new ProductTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.productNull);

            //Act
            var result = await _productServiceMock.DeleteProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task DeleteProduct_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();
            var shopDetailObject = new ShopDetailTest();

            _productRepoMock.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(productObject.product);
            _productRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _productServiceMock.DeleteProduct(1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task CreateProduct_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var productObject = new ProductTest();
            var userObject = new UserTest();

            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductRequest>()))
                .Returns(productObject.product);
            _productRepoMock.Setup(r => r.AddAsync(It.IsAny<Product>()));
            _userRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.userList);
            _notiRepoMock.Setup(r => r.AddAsync(It.IsAny<Notification>()));
            _notiRecepRepoMock.Setup(r => r.AddAsync(It.IsAny<NotificationRecepient>()));
            _stockHubMock.Setup(r => r.Clients.All.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()));
            
            //Act
            var result = await _productServiceMock.CreateProduct(productObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task CreateProduct_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var productObject = new ProductTest();

            _mapperMock.Setup(m => m.Map<Product>(It.IsAny<ProductRequest>()))
                .Throws(new Exception("failed"));

            //Act
            var result = await _productServiceMock.CreateProduct(productObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }
    }
}
