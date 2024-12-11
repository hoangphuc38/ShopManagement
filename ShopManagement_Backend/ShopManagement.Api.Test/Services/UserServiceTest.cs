using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ShopManagement.Api.Test.Fixtures;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Api.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IShopRepository> _shopRepoMock;
        private readonly Mock<IShopDetailRepository> _shopDetailRepoMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
        private readonly Mock<ResourceManager> _resourceMock;
        private readonly UserService _userServiceMock;

        public UserServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _userRepoMock = new Mock<IUserRepository>();
            _shopRepoMock = new Mock<IShopRepository>();
            _shopDetailRepoMock = new Mock<IShopDetailRepository>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
            _resourceMock = new Mock<ResourceManager>();

            _userServiceMock = new UserService(
                _mapperMock.Object,
                _userRepoMock.Object,
                _shopRepoMock.Object,
                _shopDetailRepoMock.Object,
                _loggerMock.Object,
                _memoryCacheServiceMock.Object);
        }

        [Fact]
        public async Task GetUserWithPagination_OnSuccess_ShouldReturnData()
        {
            //Arrange
            var userObject = new UserTest();
            int totalRecords = 2;

            _userRepoMock.Setup(r => r.GetUsersWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(userObject.userList);
            _mapperMock.Setup(m => m.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(userObject.userListMap);

            //Act
            var result = await _userServiceMock.GetUsersWithPagination(userObject.paginationRequest);

            int totalPages = 2 / userObject.paginationRequest.PageSize + 1;

            var res = new PaginationResponse
            {
                PageNumber = 1,
                PageSize = 10,
                TotalOfNumberRecord = 2,
                TotalOfPages = 1,
                Results = userObject.userListMap,
            };

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(res.PageNumber, response.PageNumber);
            Assert.Equal(res.PageSize, response.PageSize);
            Assert.Equal(res.TotalOfNumberRecord, response.TotalOfNumberRecord);
            Assert.Equal(res.TotalOfPages, response.TotalOfPages);
        }

        [Fact]
        public async Task GetUserWithPagination_TotalRecordsReturn0_ShouldReturnData()
        {
            //Arrange
            var userObject = new UserTest();
            int totalRecords = 0;

            _userRepoMock.Setup(r => r.GetUsersWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out totalRecords))
                .Returns(new List<User>());
            _mapperMock.Setup(m => m.Map<List<UserResponse>>(It.IsAny<List<User>>()))
                .Returns(new List<UserResponse>());

            //Act
            var result = await _userServiceMock.GetUsersWithPagination(userObject.paginationRequest);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
            Assert.IsType<PaginationResponse>(result.Data);

            var response = result.Data as PaginationResponse;

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);
            Assert.Equal(0, response.TotalOfNumberRecord);
            Assert.Equal(1, response.TotalOfPages);
        }

        [Fact]
        public async Task GetUserWithPagination_InvalidPageIndex_ShouldReturnStatusCode400()
        {
            //Arrange
            var request = new UserPaginationRequest
            {
                PageIndex = 0,
                PageSize = 10,
            };

            //Act
            var result = await _userServiceMock.GetUsersWithPagination(request);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, result.Status);
        }

        [Fact]
        public async Task GetUserWithPagination_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetUsersWithPagination(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), out It.Ref<int>.IsAny))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _userServiceMock.GetUsersWithPagination(userObject.paginationRequest);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task GetUserWithCache_OnSuccess_ShouldReturnData()
        {
            //Arrange
            var userObject = new UserTest();

            _memoryCacheServiceMock.Setup(c => c.GetCacheData(It.IsAny<string>()))
                .Returns(new BaseResponse(userObject.user));

            //Act
            var result = await _userServiceMock.GetUser(1);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task GetUserNoCache_OnSuccess_ShouldReturnData()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.user);
            _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>()))
                .Returns(userObject.userRes);
            _memoryCacheServiceMock.Setup(c => c.SetCache(It.IsAny<string>(), It.IsAny<BaseResponse>()));

            //Act
            var result = await _userServiceMock.GetUser(1);

            //Assert
            Assert.NotNull(result.Data);
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task GetUser_NotFoundUser_ShouldReturnStatusCode404()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.userNull);

            //Act
            var result = await _userServiceMock.GetUser(1);

            //Assert
            Assert.Null(result.Data);
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task GetUser_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _userServiceMock.GetUser(1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task UpdateUser_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.user);
            _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>()));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _userServiceMock.UpdateUser(1, userObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task UpdateUser_NotFoundUser_ShouldReturnStatusCode404()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.userNull);

            //Act
            var result = await _userServiceMock.UpdateUser(1, userObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task UpdateUser_ThrowException_ShouldReturnStatusCode404()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _userServiceMock.UpdateUser(1, userObject.request);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }

        [Fact]
        public async Task DeleteUser_OnSuccess_ShouldReturnStatusCode200()
        {
            //Arrange
            var userObject = new UserTest();
            var shopObject = new ShopTest();
            var shopDetailObject = new ShopDetailTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.user);
            _userRepoMock.Setup(r => r.UpdateAsync(It.IsAny<User>()));
            _shopRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Shop, bool>>>()))
                .ReturnsAsync(shopObject.shopList);
            _shopRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Shop>()));
            _shopDetailRepoMock.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ShopDetail, bool>>>()))
                .ReturnsAsync(shopDetailObject.shopDetailList);
            _shopDetailRepoMock.Setup(r => r.DeleteAsync(It.IsAny<ShopDetail>()));
            _memoryCacheServiceMock.Setup(c => c.RemoveCache(It.IsAny<string>()));

            //Act
            var result = await _userServiceMock.DeleteUser(1);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, result.Status);
        }

        [Fact]
        public async Task DeleteUser_NotFoundUser_ShouldReturnStatusCode404()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(userObject.userNull);

            //Act
            var result = await _userServiceMock.DeleteUser(1);

            //Assert
            Assert.Equal(StatusCodes.Status404NotFound, result.Status);
        }

        [Fact]
        public async Task DeleteUser_ThrowException_ShouldReturnStatusCode500()
        {
            //Arrange
            var userObject = new UserTest();

            _userRepoMock.Setup(r => r.GetFirstOrNullAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Throws(new Exception("Failed"));

            //Act
            var result = await _userServiceMock.DeleteUser(1);

            //Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, result.Status);
        }
    }
}
