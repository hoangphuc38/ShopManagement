using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Resources;

namespace ShopManagement.Api.Test.Services
{
    public class AccountServiceTest
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ITokenRepository> _tokenRepoMock;
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly Mock<ILogger<AccountService>> _loggerMock;
        private readonly Mock<ResourceManager> _resourceMock;
        private readonly AccountService _accountServiceMock;

        public AccountServiceTest()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _userRepoMock = new Mock<IUserRepository>();
            _tokenRepoMock = new Mock<ITokenRepository>();
            _roleRepoMock = new Mock<IRoleRepository>();
            _loggerMock = new Mock<ILogger<AccountService>>();
            _resourceMock = new Mock<ResourceManager>();

            _accountServiceMock = new AccountService(
                _tokenServiceMock.Object,
                _userRepoMock.Object,
                _tokenRepoMock.Object,
                _roleRepoMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task AddRole_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var roleObject = new RoleTest();

            _roleRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roleObject.roleNull);
            _roleRepoMock.Setup(r => r.AddAsync(It.IsAny<Role>()));

            //Act
            var result = await _accountServiceMock.AddRole("admin");

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task AddRole_RoleExist_ShouldReturnStatusCode400()
        {
            //Arrange
            var roleObject = new RoleTest();

            _roleRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roleObject.role);

            //Act
            var result = await _accountServiceMock.AddRole("admin");

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task AddRole_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var roleObject = new RoleTest();

            _roleRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _accountServiceMock.AddRole("admin");

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task RefreshToken_OnSuccess_ShouldReturnData()
        {
            //Arrange
            var accountObject = new AccountTest();
            var userObject = new UserTest();
            var roleObject = new RoleTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.token);
            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.user);
            _roleRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roleObject.role);
            _tokenServiceMock.Setup(r => r.GenerateToken(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("newAccessToken");

            //Act
            var result = await _accountServiceMock.RefreshToken(accountObject.tokenRequest);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ShouldReturnStatusCode400()
        {
            //Arrange
            var accountObject = new AccountTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.tokenNull);

            //Act
            var result = await _accountServiceMock.RefreshToken(accountObject.tokenRequest);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task RefreshToken_NotFoundUser_ShouldReturnStatusCode400()
        {
            //Arrange
            var accountObject = new AccountTest();
            var userObject = new UserTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.token);
            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.userNull);

            //Act
            var result = await _accountServiceMock.RefreshToken(accountObject.tokenRequest);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task RefreshToken_NotFoundRole_ShouldReturnStatusCode500()
        {
            //Arrange
            var accountObject = new AccountTest();
            var userObject = new UserTest();
            var roleObject = new RoleTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.token);
            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.user);
            _roleRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roleObject.roleNull);

            //Act
            var result = await _accountServiceMock.RefreshToken(accountObject.tokenRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task RefreshToken_ThrowException_ShouldReturnStatusCode400()
        {
            //Arrange
            var accountObject = new AccountTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _accountServiceMock.RefreshToken(accountObject.tokenRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task Logout_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var accountObject = new AccountTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.token);
            _tokenRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Token>()));

            //Act
            var result = await _accountServiceMock.Logout(1);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task Logout_IsAlreadyLogout_ShouldReturnStatusCode400()
        {
            //Arrange
            var accountObject = new AccountTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .ReturnsAsync(accountObject.tokenNull);

            //Act
            var result = await _accountServiceMock.Logout(1);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task Logout_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var accountObject = new AccountTest();

            _tokenRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<Token, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _accountServiceMock.Logout(1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }
    }
}
