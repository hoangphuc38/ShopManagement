using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class ShopService : IShopService
    {
        private readonly IMapper _mapper;
        private readonly IShopRepository _shopRepo;
        private readonly IUserRepository _userRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly ILogger<ShopService> _logger;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ResourceManager _resource;

        public ShopService(
            IMapper mapper,
            IShopRepository shopRepo,
            IUserRepository userRepo,
            IShopDetailRepository shopDetailRepo,
            ILogger<ShopService> logger,
            IMemoryCacheService memoryCacheService)
        {
            _mapper = mapper;
            _shopRepo = shopRepo;
            _userRepo = userRepo;
            _shopDetailRepo = shopDetailRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.ShopMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> GetAll()
        {
            try
            {
                _logger.LogInformation($"[GetAllShop] Start to get all shops.");
                var response = _memoryCacheService.GetCacheData("ShopList");

                if (response == null)
                {
                    var shopList = await _shopRepo.GetAllShops();

                    var shopListMapper = _mapper.Map<List<ShopResponse>>(shopList);

                    response = new BaseResponse(shopListMapper);

                    _memoryCacheService.SetCache("ShopList", response);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllShop] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetListFailed") ?? "");
            }
        }

        public async Task<BaseResponse> GetShopOfUser(int userID)
        {
            try
            {
                _logger.LogInformation($"[GetShopOfUser] Start to get shops of user with id: {userID}.");
                var response = _memoryCacheService.GetCacheData($"Shop_{userID}");

                if (response == null)
                {
                    var shopList = await _shopRepo.GetAllAsync(c => c.UserId == userID && !c.IsDeleted);
                    var responseShopList = new List<ShopResponse>();

                    if (shopList == null)
                    {
                        return new BaseResponse(
                            StatusCodes.Status404NotFound,
                            _resource.GetString("NotFoundShop") ?? "");
                    }

                    foreach (var shop in shopList)
                    {
                        var shopMapper = _mapper.Map<ShopResponse>(shop);
                        var user = await _userRepo.GetFirstOrNullAsync(c => c.Id == userID && !c.IsDeleted);

                        if (user == null)
                        {
                            return new BaseResponse(
                                StatusCodes.Status404NotFound,
                                _resource.GetString("NotFoundUser") ?? "");
                        }

                        shopMapper.OwnerName = user.FullName;

                        responseShopList.Add(shopMapper);
                    }

                    response = new BaseResponse(responseShopList);

                    _memoryCacheService.SetCache($"Shop_{userID}", response);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetShopOfUser] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetShopFailed") ?? "");
            }
        }

        public async Task<BaseResponse> UpdateShop(int shopID, ShopRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateShop] Start to update shop with id: {shopID} " +
                    $"ShopName: {request.ShopName}, ShopAddress: {request.ShopAddress}");
                var shop = await _shopRepo.GetFirstOrNullAsync(c => c.ShopId == shopID && !c.IsDeleted);

                if (shop == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundShop") ?? "");
                }

                shop.ShopName = request.ShopName;
                shop.ShopAddress = request.ShopAddress;

                await _shopRepo.UpdateAsync(shop);

                _memoryCacheService.RemoveCache($"Shop_{shop.UserId}");
                _memoryCacheService.RemoveCache("ShopList");

                return new BaseResponse(_resource.GetString("UpdateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateShop] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("UpdateFailed") ?? "");
            }
        }

        public async Task<BaseResponse> CreateShop(int userID, ShopRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateShop] Start to create shop of user with id: {userID} " +
                    $"ShopName: {request.ShopName}, ShopAddress: {request.ShopAddress}");
                var user = await _userRepo.GetFirstOrNullAsync(c => c.Id == userID && !c.IsDeleted);

                if (user == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundUser") ?? "");
                }

                var shop = _mapper.Map<Shop>(request);
                shop.UserId = userID;
                shop.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                shop.IsDeleted = false;

                await _shopRepo.AddAsync(shop);

                _memoryCacheService.RemoveCache($"Shop_{userID}");
                _memoryCacheService.RemoveCache("ShopList");

                return new BaseResponse(_resource.GetString("CreateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateShop] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("CreateFailed") ?? "");
            }
        }

        public async Task<BaseResponse> DeleteShop(int shopID)
        {
            try
            {
                _logger.LogInformation($"[DeleteShop] Start to delete shop with id: {shopID}");
                var shop = await _shopRepo.GetFirstOrNullAsync(c => c.ShopId == shopID && !c.IsDeleted);

                if (shop == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundShop") ?? "");
                }

                shop.IsDeleted = true;
                await _shopRepo.UpdateAsync(shop);

                var detailList = await _shopDetailRepo.GetAllAsync(c => c.ShopId == shopID && !c.IsDeleted);
                foreach (var detail in detailList)
                {
                    detail.IsDeleted = true;
                    await _shopDetailRepo.UpdateAsync(detail);
                }

                _memoryCacheService.RemoveCache($"Shop_{shop.UserId}");
                _memoryCacheService.RemoveCache("ShopList");

                return new BaseResponse(_resource.GetString("DeleteSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteShop] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("DeleteFailed") ?? "");
            }
        }
    }
}
