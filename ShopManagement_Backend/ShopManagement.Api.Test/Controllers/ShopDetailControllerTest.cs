using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopManagement_Backend_Application.Models.ShopDetail;

namespace ShopManagement.Api.Test.Controllers
{
    public class ShopDetailControllerTest
    {
        private readonly Mock<IShopDetailService> _shopDetailServiceMock;
        private readonly ShopDetailController _shopDetailController;

        public ShopDetailControllerTest()
        {
            _shopDetailServiceMock = new Mock<IShopDetailService>();
            _shopDetailController = new ShopDetailController(_shopDetailServiceMock.Object);
        }

        [Fact]
        public async Task GetShopDetailWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var shopDetailObject = new ShopDetailTest();
            var response = new BaseResponse(shopDetailObject.ShopPagination);
            var request = new ShopDetailPaginationRequest();

            _shopDetailServiceMock.Setup(
                service => service.GetShopDetailWithPagination(It.IsAny<int>(), It.IsAny<ShopDetailPaginationRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopDetailController.GetShopDetailWithPagination(1, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task CreateShopDetail_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            ShopDetailRequest request = new ShopDetailRequest { 
                ShopId = 1, ProductId = 1, Quantity = 2
            };
            var response = new BaseResponse("Create new shop detail successfully");

            _shopDetailServiceMock.Setup(service => service.CreateDetail(It.IsAny<ShopDetailRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopDetailController.CreateDetail(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task CreateShopDetail_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            ShopDetailRequest request = new ShopDetailRequest
            {
                ShopId = 1,
                ProductId = 1,
                Quantity = 2
            };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create new shop detail");

            _shopDetailServiceMock.Setup(service => service.CreateDetail(It.IsAny<ShopDetailRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopDetailController.CreateDetail(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task DeleteShopDetail_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var response = new BaseResponse("Delete shop detail successfully");

            _shopDetailServiceMock.Setup(service => service.DeleteDetail(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopDetailController.DeleteProduct(1, 1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task DeleteShopDetail_AlreadyDeleted_ShouldReturnStatusCode500()
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete shop detail successfully");

            _shopDetailServiceMock.Setup(service => service.DeleteDetail(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _shopDetailController.DeleteProduct(1, 1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task UpdateDetailShop_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            ShopDetailRequest request = new ShopDetailRequest
            {
                ShopId = 1,
                ProductId = 1,
                Quantity = 2
            };
            var response = new BaseResponse("Update shop detail successfully");

            _shopDetailServiceMock.Setup(service => service.UpdateDetail(It.IsAny<ShopDetailRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _shopDetailController.UpdateDetail(request);

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
            ShopDetailRequest request = new ShopDetailRequest
            {
                ShopId = 1,
                ProductId = 1,
                Quantity = 2
            };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update shop detail successfully");

            _shopDetailServiceMock.Setup(service => service.UpdateDetail(It.IsAny<ShopDetailRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _shopDetailController.UpdateDetail(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }
    }
}
