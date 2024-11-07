using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

namespace ShopManagement_Backend_Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IShopRepository _shopRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IMapper mapper,
            IUserRepository userRepo,
            IShopRepository shopRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<UserService> logger)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _shopRepo = shopRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
        }

        public BaseResponse GetAllUser()
        {
            try
            {
                _logger.LogInformation($"[GetAllUser] Start to get all users");
                var userList = _userRepo.GetAllAsync(c => !c.IsDeleted);
                var responseList = _mapper.Map<List<UserResponse>>(userList);

                return new BaseResponse(responseList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllOfUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all of user");
            }
        }

        public BaseResponse GetUser(int id)
        {
            try
            {
                _logger.LogInformation($"[GetUser] Start to get user with id: {id}");
                var user = _userRepo.GetFirstAsync(c => c.Id == id && !c.IsDeleted);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
                }

                var response = _mapper.Map<UserResponse>(user);

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get user");
            }
        }

        public BaseResponse CreateUser(UserRequest user)
        {
            try
            {
                _logger.LogInformation($"[CreateUser] Start to create a user. " +
                    $"UserName: {user.UserName}, FullName: {user.FullName}, Address: {user.Address}");
                var newUser = _mapper.Map<User>(user);
                newUser.IsDeleted = false;
                newUser.SignUpDate = DateOnly.FromDateTime(DateTime.Now);

                _userRepo.AddAsync(newUser);

                return new BaseResponse("Create user successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create a user");
            }
        }

        public BaseResponse UpdateUser(int id, UserRequest user)
        {
            try
            {
                _logger.LogInformation($"[UpdateUser]: Start to update a user with id: {id} " +
                    $"UserName: {user.UserName}, FullName: {user.FullName}, Address: {user.Address}");
                var userUpdate = _userRepo.GetFirstAsync(c => c.Id == id && !c.IsDeleted);

                if (userUpdate == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
                }

                userUpdate.FullName = user.FullName;
                userUpdate.UserName = user.UserName;
                userUpdate.Address = user.Address;

                _userRepo.UpdateAsync(userUpdate);

                var response = _mapper.Map<UserResponse>(userUpdate);

                return new BaseResponse("Update user successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update a user");
            }
        }

        public BaseResponse DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteUser] Start to delete a user with id: {id}");
                var user = _userRepo.GetFirstAsync(c => c.Id == id && !c.IsDeleted);

                if (user == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Not found user");
                }

                user.IsDeleted = true;
                _userRepo.UpdateAsync(user);

                var shopList = _shopRepo.GetAllAsync(c => c.UserId == user.Id && !c.IsDeleted);
                foreach (var shop in shopList)
                {
                    shop.IsDeleted = true;
                    _shopRepo.UpdateAsync(shop);

                    var shopDetail = _shopDetailRepo.GetAllAsync(c => c.ShopId == shop.ShopId && !c.IsDeleted);
                    foreach (var detail in shopDetail)
                    {
                        detail.IsDeleted = true;
                        _shopDetailRepo.DeleteAsync(detail);
                    }
                }

                return new BaseResponse("Delete user successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteUser] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete a user");
            }
        }
    }
}
