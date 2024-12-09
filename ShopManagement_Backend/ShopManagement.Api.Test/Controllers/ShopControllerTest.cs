using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Application.Models.Shop;

namespace ShopManagement.Api.Test.Controllers
{
    public class ShopControllerTest
    {
        private readonly Mock<IShopService> _shopServiceMock;
        private readonly ShopController _shopController;

        public ShopControllerTest()
        {
            _shopServiceMock = new Mock<IShopService>();
            _shopController = new ShopController(_shopServiceMock.Object);
        }

        [Fact]
        public async Task GetShopWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            var response = new BaseResponse(shopObject.ShopPagination);
            var request = new ShopPaginationRequest();

            _shopServiceMock.Setup(
                service => service.GetShopsWithPagination(It.IsAny<ShopPaginationRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.GetShopsWithPagination(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task GetShopOfUser_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopObject = new ShopTest();
            var response = new BaseResponse(shopObject.ShopPagination);
            var pagination = new ShopPaginationRequest();

            _shopServiceMock.Setup(service => service.GetShopOfUser(It.IsAny<int>(), It.IsAny<ShopPaginationRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.GetShopOfUser(1, pagination);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task CreateShop_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            ShopRequest request = new ShopRequest { ShopName = "Test", ShopAddress = "Test" };
            var response = new BaseResponse("Create new shop successfully");

            _shopServiceMock.Setup(service => service.CreateShop(It.IsAny<int>(), It.IsAny<ShopRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.CreateShop(1, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task CreateProduct_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            ShopRequest request = new ShopRequest { ShopName = "Test" };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create new shop");

            _shopServiceMock.Setup(service => service.CreateShop(It.IsAny<int>(), It.IsAny<ShopRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.CreateShop(1, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteShop_OnSuccess_ShouldReturnStatusCode200(int shopID)
        {
            //Arrange
            var response = new BaseResponse("Delete shop successfully");

            _shopServiceMock.Setup(service => service.DeleteShop(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.DeleteShop(shopID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteProduct_AlreadyDeleted_ShouldReturnStatusCode500(int shopID)
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete shop successfully");

            _shopServiceMock.Setup(service => service.DeleteShop(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopController.DeleteShop(shopID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateProduct_OnSuccess_ShouldReturnStatusCode200(int shopID)
        {
            //Arrange
            ShopRequest request = new ShopRequest { ShopName = "Test", ShopAddress = "Test" };
            var response = new BaseResponse("Update shop successfully");

            _shopServiceMock.Setup(service => service.UpdateShop(It.IsAny<int>(), It.IsAny<ShopRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _shopController.UpdateShop(shopID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateProduct_Error_ShouldReturnStatusCode500(int shopID)
        {
            //Arrange
            ShopRequest request = new ShopRequest { ShopName = "Test", ShopAddress = "Test" };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update shop successfully");

            _shopServiceMock.Setup(service => service.UpdateShop(It.IsAny<int>(), It.IsAny<ShopRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _shopController.UpdateShop(shopID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }
    }
}
