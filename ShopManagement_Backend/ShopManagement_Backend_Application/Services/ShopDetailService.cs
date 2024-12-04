using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class ShopDetailService : IShopDetailService
    {
        private readonly IMapper _mapper;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly IProductRepository _productRepo;
        private readonly IShopRepository _shopRepo;
        private readonly ILogger<ShopDetailService> _logger;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ResourceManager _resource;

        public ShopDetailService(
            IMapper mapper,
            IShopDetailRepository shopDetailRepo,
            IProductRepository productRepo,
            IShopRepository shopRepo,
            ILogger<ShopDetailService> logger,
            IMemoryCacheService memoryCacheService)
        {
            _mapper = mapper;
            _shopDetailRepo = shopDetailRepo;
            _productRepo = productRepo;
            _shopRepo = shopRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.ShopDetailMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> GetShopDetailWithPagination(int id, ShopDetailPaginationRequest request)
        {
            try
            {
                _logger.LogInformation($"[GetAllOfShop] Start to get all products in shop with id: {id}");

                if (request.PageIndex < 1 || request.PageSize < 1)
                {
                    return new BaseResponse(
                        StatusCodes.Status400BadRequest,
                        _resource.GetString("InvalidPage") ?? "");
                }

                string filter = string.Empty;
                int totalRecords;

                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    filter = $" AND p.ProductName LIKE '%{request.SearchText}%' ";
                }

                string sortOrder = request.Sort == SortType.Ascending ? SortType.AscendingSort : SortType.DescendingSort;

                string sort = $" ORDER BY {request.Column} {sortOrder} ";

                var shopDetail = _shopDetailRepo.GetShopDetailWithPagination(
                    id,
                    request.PageIndex, request.PageSize, sort, filter, out totalRecords);

                if (shopDetail.Count() == 0)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundProduct") ?? "");
                }

                int totalPages = totalRecords / request.PageSize + 1;

                var shopDetailMapper = _mapper.Map<IEnumerable<ShopDetailResponse>>(shopDetail);

                var response = new PaginationResponse
                {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalOfNumberRecord = totalRecords,
                    TotalOfPages = totalPages,
                    Results = shopDetailMapper
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllOfShop] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetProductsFailed") ?? "");
            }
        }

        public async Task<BaseResponse> UpdateDetail(ShopDetailRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateDetail] Start to update detail in shop. " +
                    $"ShopID: {request.ShopId}, ProductID: {request.ProductId}, Quantity: {request.Quantity}");
                var product = await _productRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId && !c.IsDeleted);

                var shop = await _shopRepo.GetFirstOrNullAsync(c => c.ShopId == request.ShopId && !c.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundProduct") ?? "");
                }

                if (shop == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundShop") ?? "");
                }

                var detail = await _shopDetailRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId
                                     && c.ShopId == request.ShopId
                                     && !c.IsDeleted);

                if (detail == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("DetailNotExist") ?? "");
                }

                detail.Quantity = request.Quantity;

                await _shopDetailRepo.UpdateAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{request.ShopId}");

                return new BaseResponse(_resource.GetString("UpdateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateDetail] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("UpdateFailed") ?? "");
            }
        }

        public async Task<BaseResponse> DeleteDetail(int shopID, int productID)
        {
            try
            {
                _logger.LogInformation($"[DeleteDetail] Start to delete detail with productid {productID} in shop with id: {shopID}");
                var detail = await _shopDetailRepo.GetFirstOrNullAsync(c => c.ProductId == productID
                                 && c.ShopId == shopID
                                 && !c.IsDeleted);

                if (detail == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("DetailNotExist") ?? "");
                }

                detail.IsDeleted = true;

                await _shopDetailRepo.DeleteAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{shopID}");

                return new BaseResponse(_resource.GetString("DeleteSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteDetail] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("DeleteFailed") ?? "");
            }
        }

        public async Task<BaseResponse> CreateDetail(ShopDetailRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateDetail] Start to create detail in shop. " +
                    $"ShopID: {request.ShopId}, ProductID: {request.ProductId}, Quantity: {request.Quantity}");
                var product = await _productRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId && !c.IsDeleted);

                var shop = await _shopRepo.GetFirstOrNullAsync(c => c.ShopId == request.ShopId && !c.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound, 
                        _resource.GetString("NotFoundProduct") ?? "");
                }

                if (shop == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundShop") ?? "");
                }

                var detail = await _shopDetailRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId
                                     && c.ShopId == request.ShopId
                                     && !c.IsDeleted);

                //If detail not exist, create new one
                if (detail == null)
                {
                    var shopDetail = _mapper.Map<ShopDetail>(request);
                    shopDetail.IsDeleted = false;

                    await _shopDetailRepo.AddAsync(shopDetail);

                    return new BaseResponse(_resource.GetString("CreateSuccess") ?? "");
                }

                //If exist, add quantity for detail
                detail.Quantity += request.Quantity;

                await _shopDetailRepo.UpdateAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{request.ShopId}");

                return new BaseResponse(_resource.GetString("CreateWhenExist") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateDetail] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("CreateFailed") ?? "");
            }
        }
    }
}
