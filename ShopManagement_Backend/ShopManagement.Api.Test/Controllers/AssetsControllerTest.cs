using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Application.Models.Asset;

namespace ShopManagement.Api.Test.Controllers
{
    public class AssetsControllerTest
    {
        private readonly Mock<IImageService> _imageServiceMock;
        private readonly AssetsController _assetController;

        public AssetsControllerTest()
        {
            _imageServiceMock = new Mock<IImageService>();
            _assetController = new AssetsController(_imageServiceMock.Object);
        }

        [Fact]
        public async Task UploadAsync_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var assetObject = new AssetTest();
            var response = new BaseResponse(assetObject.assetRes);

            _imageServiceMock.Setup(
                service => service.UploadAsync(It.IsAny<AssetRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _assetController.UploadAsync(assetObject.assetRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task UploadAsync_Error_ShouldReturnStatusCode400()
        {
            //Arrange
            var assetObject = new AssetTest();
            var response = new BaseResponse(StatusCodes.Status400BadRequest, "Failed");

            _imageServiceMock.Setup(
                service => service.UploadAsync(It.IsAny<AssetRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _assetController.UploadAsync(assetObject.assetRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status400BadRequest, returnResult.Status);
        }

        [Fact]
        public async Task DeleteAsync_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var assetObject = new AssetTest();
            var response = new BaseResponse("Delete successfully");

            _imageServiceMock.Setup(
                service => service.DeleteAsync(It.IsAny<AssetDeleteRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _assetController.DeleteAsync(assetObject.deleteRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task DeleteAsync_Error_ShouldReturnStatusCode400()
        {
            //Arrange
            var assetObject = new AssetTest();
            var response = new BaseResponse(StatusCodes.Status400BadRequest, "Failed");

            _imageServiceMock.Setup(
                service => service.DeleteAsync(It.IsAny<AssetDeleteRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _assetController.DeleteAsync(assetObject.deleteRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status400BadRequest, returnResult.Status);
        }
    }
}
