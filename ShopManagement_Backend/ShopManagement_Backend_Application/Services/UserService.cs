using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IShopRepository _shopRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly ILogger<UserService> _logger;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ResourceManager _resource;

        public UserService(
            IMapper mapper,
            IUserRepository userRepo,
            IShopRepository shopRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<UserService> logger,
            IMemoryCacheService memoryCacheService)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _shopRepo = shopRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.UserMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> GetUsersWithPagination(UserPaginationRequest request)
        {
            try
            {
                _logger.LogInformation($"[GetAllUser] Start to get all users");

                // validate request
                if (request.PageIndex < 1 || request.PageSize < 1)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("InvalidPage") ?? "");
                }

                // logic
                string filter = string.Empty;
                int totalRecords;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    filter = $" AND FullName LIKE '%{request.SearchText}%' ";
                }

                string sortOrder = request.Sort == SortType.Ascending ? SortType.AscendingSort : SortType.DescendingSort;

                string sort = $" ORDER BY {request.Column} {sortOrder} ";

                var userList = _userRepo.GetUsersWithPagination(
                    request.PageIndex, request.PageSize, sort, filter, out totalRecords);

                if (totalRecords == 0)
                {
                    return new BaseResponse(new PaginationResponse(request.PageIndex, request.PageSize, userList));
                }

                int totalPages = totalRecords / request.PageSize + 1;

                var userMapperList = _mapper.Map<List<UserResponse>>(userList);

                var response = new PaginationResponse
                {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalOfNumberRecord = totalRecords,
                    TotalOfPages = totalPages,
                    Results = userMapperList
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllOfUser] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetAllFailed") ?? "");
            }
        }

        public async Task<BaseResponse> GetUser(int id)
        {
            try
            {
                _logger.LogInformation($"[GetUser] Start to get user with id: {id}");
                var response = _memoryCacheService.GetCacheData($"User_{id}");

                if (response == null)
                {
                    var user = await _userRepo.GetFirstOrNullAsync(c => c.Id == id && !c.IsDeleted);

                    if (user == null)
                    {
                        return new BaseResponse(
                            StatusCodes.Status404NotFound,
                            _resource.GetString("NotFoundUser") ?? "");
                    }

                    var userMapper = _mapper.Map<UserResponse>(user);

                    response = new BaseResponse(userMapper);

                    _memoryCacheService.SetCache($"User_{id}", response);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetUser] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetUserFailed") ?? "");
            }
        }

        public async Task<BaseResponse> UpdateUser(int id, UserRequest user)
        {
            try
            {
                _logger.LogInformation($"[UpdateUser]: Start to update a user with id: {id} " +
                    $"UserName: {user.UserName}, FullName: {user.FullName}, Address: {user.Address}");
                var userUpdate = await _userRepo.GetFirstOrNullAsync(c => c.Id == id && !c.IsDeleted);

                if (userUpdate == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundUser") ?? "");
                }

                userUpdate.FullName = user.FullName;
                userUpdate.UserName = user.UserName;
                userUpdate.Address = user.Address;

                await _userRepo.UpdateAsync(userUpdate);

                _memoryCacheService.RemoveCache("UserList");
                _memoryCacheService.RemoveCache($"User_{id}");

                return new BaseResponse(_resource.GetString("UpdateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateUser] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("UpdateFailed") ?? "");
            }
        }

        public async Task<BaseResponse> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteUser] Start to delete a user with id: {id}");
                var user = await _userRepo.GetFirstOrNullAsync(c => c.Id == id && !c.IsDeleted);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundUser") ?? "");
                }

                user.IsDeleted = true;
                await _userRepo.UpdateAsync(user);

                var shopList = await _shopRepo.GetAllAsync(c => c.UserId == user.Id && !c.IsDeleted);
                foreach (var shop in shopList)
                {
                    shop.IsDeleted = true;
                    await _shopRepo.UpdateAsync(shop);

                    var shopDetail = await _shopDetailRepo.GetAllAsync(c => c.ShopId == shop.ShopId && !c.IsDeleted);
                    foreach (var detail in shopDetail)
                    {
                        detail.IsDeleted = true;
                        await _shopDetailRepo.DeleteAsync(detail);
                    }
                }

                _memoryCacheService.RemoveCache("UserList");
                _memoryCacheService.RemoveCache($"User_{id}");

                return new BaseResponse(_resource.GetString("DeleteSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteUser] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("DeleteFailed") ?? "");
            }
        }
    }
}
