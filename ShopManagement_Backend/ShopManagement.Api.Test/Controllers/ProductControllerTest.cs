using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;

namespace ShopManagement.Api.Test.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _productController;

        public ProductControllerTest()
        {
            _productServiceMock = new Mock<IProductService>();
            _productController = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetProductWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var productObject = new ProductTest();
            var response = new BaseResponse(productObject.ProductPagination);
            var request = new ProductPaginationRequest();

            _productServiceMock.Setup(
                service => service.GetProductsWithPagination(It.IsAny<ProductPaginationRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.GetProductsWithPagination(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Theory]
        [ClassData(typeof(ProductTest))]
        public async Task GetDetailProduct_OnSuccess_ShouldReturnBaseResponse(int productID)
        {
            //Arrange
            var productObject = new ProductTest();
            var response = new BaseResponse(productObject.Products[0]);

            _productServiceMock.Setup(service => service.GetDetailProduct(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.GetDetailProduct(productID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetDetailProduct_NotFound_ShouldReturnStatusCode500(int productID)
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get product");

            _productServiceMock.Setup(service => service.GetDetailProduct(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.GetDetailProduct(productID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
            Assert.Null(returnResult.Data);
        }

        [Fact]
        public async Task CreateProduct_OnSuccess_ShouldReturnStatusCode200()
        {
            ////Arrange
            ProductRequest request = new ProductRequest { 
                ProductName = "Test", 
                Price = 100,
                ImageUrl = "testurl",
            };
            var response = new BaseResponse("Create new product successfully");

            _productServiceMock.Setup(service => service.CreateProduct(It.IsAny<ProductRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.CreateProduct(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task CreateProduct_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            ProductRequest request = new ProductRequest
            {
                ProductName = "Test",
                Price = 100,
                ImageUrl = "testurl",
            };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create new product");

            _productServiceMock.Setup(service => service.CreateProduct(It.IsAny<ProductRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.CreateProduct(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteProduct_OnSuccess_ShouldReturnStatusCode200(int productID)
        {
            //Arrange
            var response = new BaseResponse("Delete product successfully");

            _productServiceMock.Setup(service => service.DeleteProduct(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.DeleteProduct(productID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteProduct_AlreadyDeleted_ShouldReturnStatusCode500(int productID)
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete product successfully");

            _productServiceMock.Setup(service => service.DeleteProduct(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _productController.DeleteProduct(productID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateProduct_OnSuccess_ShouldReturnStatusCode200(int productID)
        {
            //Arrange
            ProductRequest request = new ProductRequest
            {
                ProductName = "Test",
                Price = 100,
                ImageUrl = "testurl",
            };
            var response = new BaseResponse("Update product successfully");

            _productServiceMock.Setup(service => service.UpdateProduct(It.IsAny<int>(), It.IsAny<ProductRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _productController.UpdateProduct(productID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateProduct_Error_ShouldReturnStatusCode500(int productID)
        {
            //Arrange
            ProductRequest request = new ProductRequest
            {
                ProductName = "Test",
                Price = 100,
                ImageUrl = "testurl",
            };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update product successfully");

            _productServiceMock.Setup(service => service.UpdateProduct(It.IsAny<int>(), It.IsAny<ProductRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _productController.UpdateProduct(productID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }
    }   
}
