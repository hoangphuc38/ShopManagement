using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_API.Controllers;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Application.Models.Token;

namespace ShopManagement.Api.Test.Controllers
{
    public class AccountControllerTest
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _accountController;

        public AccountControllerTest()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountController = new AccountController(_accountServiceMock.Object);
        }

        [Fact]
        public async Task Register_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse("Success");

            _accountServiceMock.Setup(
                service => service.Register(It.IsAny<RegisterUser>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Register(accountObject.register);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task Register_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to register");

            _accountServiceMock.Setup(service => service.Register(It.IsAny<RegisterUser>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Register(accountObject.register);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task Login_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse(accountObject.loginRes);

            _accountServiceMock.Setup(
                service => service.Login(It.IsAny<LoginUser>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Login(accountObject.loginRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task Login_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to login");

            _accountServiceMock.Setup(service => service.Login(It.IsAny<LoginUser>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Login(accountObject.loginRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task RefreshToken_OnSuccess_ShouldReturnBaseResponse()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse(accountObject.tokenResponse);

            _accountServiceMock.Setup(
                service => service.RefreshToken(It.IsAny<RefreshTokenRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.RefreshToken(accountObject.tokenRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
            Assert.NotNull(returnResult);
        }

        [Fact]
        public async Task RefreshToken_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var accountObject = new AccountTest();
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to refresh token");

            _accountServiceMock.Setup(service => service.RefreshToken(It.IsAny<RefreshTokenRequest>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.RefreshToken(accountObject.tokenRequest);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task AddRole_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var response = new BaseResponse("Add role succesfully");

            _accountServiceMock.Setup(
                service => service.AddRole(It.IsAny<string>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.AddRole("admin");

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task AddRole_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to add role");

            _accountServiceMock.Setup(service => service.AddRole(It.IsAny<string>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.AddRole("admin");

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }

        [Fact]
        public async Task Logout_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var response = new BaseResponse("Logout successfully");

            _accountServiceMock.Setup(
                service => service.Logout(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Logout(1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, returnResult.Status);
        }

        [Fact]
        public async Task Logout_Error_ShouldReturnStatusCode500()
        {
            //Arrange
            var response = new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to logout");

            _accountServiceMock.Setup(service => service.Logout(It.IsAny<int>()))
                .ReturnsAsync(response);

            //Act
            var result = await _accountController.Logout(1);

            //Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnResult = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.Equal(StatusCodes.Status500InternalServerError, returnResult.Status);
        }
    }
}
