using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Application.Models.User;

namespace ShopManagement.Api.Test.Controllers
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetUserWithPagination_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var userObject = new UserTest();
            var response = new BaseResponse(userObject.UserPagination);
            var request = new UserPaginationRequest();

            _userServiceMock.Setup(
                service => service.GetUsersWithPagination(It.IsAny<UserPaginationRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _userController.GetUsersWithPagination(request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task GetUser_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var userObject = new UserTest();
            var response = new BaseResponse(userObject.userRes);

            _userServiceMock.Setup(service => service.GetUser(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _userController.GetUser(1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task GetUser_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get user");

            _userServiceMock.Setup(service => service.GetUser(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _userController.GetUser(1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteUser_OnSuccess_ShouldReturnStatusCode200(int userID)
        {
            //Arrange
            var response = new BaseResponse("Delete user successfully");

            _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _userController.DeleteUser(userID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteUser_Error_ShouldReturnStatusCode500(int userID)
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete user successfully");

            _userServiceMock.Setup(service => service.DeleteUser(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _userController.DeleteUser(userID);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateUser_OnSuccess_ShouldReturnStatusCode200(int userID)
        {
            //Arrange
            UserRequest request = new UserRequest { 
                UserName = "Test", 
                Password = "Test",
                Address = "ABC",
                FullName = "Test",
            };
            var response = new BaseResponse("Update user successfully");

            _userServiceMock.Setup(service => service.UpdateUser(It.IsAny<int>(), It.IsAny<UserRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _userController.UpdateUser(userID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task UpdateProduct_Error_ShouldReturnStatusCode500(int userID)
        {
            //Arrange
            UserRequest request = new UserRequest
            {
                UserName = "Test",
                Password = "Test",
                Address = "ABC",
                FullName = "Test",
            };
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update user successfully");

            _userServiceMock.Setup(service => service.UpdateUser(It.IsAny<int>(), It.IsAny<UserRequest>()))
                .ReturnsAsync(response);

            //Action
            var result = await _userController.UpdateUser(userID, request);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }
    }
}
