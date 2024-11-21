using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement.Api.Test.Services
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IShopDetailRepository> _shopDetailRepoMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly ProductService _productServiceMock;
        
        public ProductServiceTest()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _shopDetailRepoMock = new Mock<IShopDetailRepository>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductService>>();

            _productServiceMock = new ProductService(
                _mapperMock.Object, 
                _productRepoMock.Object,
                _shopDetailRepoMock.Object,
                _loggerMock.Object,
                _memoryCacheServiceMock.Object);
        }

        [Fact]
        public void GetAllWithCacheNotNull_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var productObject = new ProductTest();
            var response = new BaseResponse(productObject.Products);
            var productList = productObject.ProductList;

            _memoryCacheServiceMock.Setup(service => service.GetCacheData(It.IsAny<string>()))
                .Returns(response);

            //Act
            var result = _productServiceMock.GetAll();

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }
    }
}
