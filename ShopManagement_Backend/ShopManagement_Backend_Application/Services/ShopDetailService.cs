﻿using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;

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
        }

        public BaseResponse GetAllOfShop(int id)
        {
            try
            {
                _logger.LogInformation($"[GetAllOfShop] Start to get all products in shop with id: {id}");

                var response = _memoryCacheService.GetCacheData($"ShopDetail_{id}");

                if (response == null)
                {
                    var shopDetail = _shopDetailRepo.GetAllAsyncByShopID(id);
                    var shopDetailMapper = _mapper.Map<IEnumerable<ShopDetailResponse>>(shopDetail);

                    response = new BaseResponse(shopDetailMapper);

                    _memoryCacheService.SetCache($"ShopDetail_{id}", response);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllOfShop] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to get all of shop");
            }
        }

        public BaseResponse UpdateDetail(ShopDetailRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateDetail] Start to update detail in shop. " +
                    $"ShopID: {request.ShopId}, ProductID: {request.ProductId}, Quantity: {request.Quantity}");
                var product = _productRepo.GetFirstAsync(c => c.ProductId == request.ProductId && !c.IsDeleted);

                var shop = _shopRepo.GetFirstAsync(c => c.ShopId == request.ShopId && !c.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                if (shop == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
                }

                var detail = _shopDetailRepo.GetFirstAsync(c => c.ProductId == request.ProductId
                                     && c.ShopId == request.ShopId
                                     && !c.IsDeleted);

                if (detail == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
                }

                detail.Quantity = request.Quantity;

                _shopDetailRepo.UpdateAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{request.ShopId}");

                return new BaseResponse("Update detail successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateDetail] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to update detail");
            }
        }

        public BaseResponse DeleteDetail(int shopID, int productID)
        {
            try
            {
                _logger.LogInformation($"[DeleteDetail] Start to delete detail with productid {productID} in shop with id: {shopID}");
                var detail = _shopDetailRepo.GetFirstAsync(c => c.ProductId == productID
                                 && c.ShopId == shopID
                                 && !c.IsDeleted);

                if (detail == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
                }

                detail.IsDeleted = true;

                _shopDetailRepo.DeleteAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{shopID}");

                return new BaseResponse("Delete detail successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteDetail] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to delete detail");
            }
        }

        public BaseResponse CreateDetail(ShopDetailRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateDetail] Start to create detail in shop. " +
                    $"ShopID: {request.ShopId}, ProductID: {request.ProductId}, Quantity: {request.Quantity}");
                var product = _productRepo.GetFirstAsync(c => c.ProductId == request.ProductId && !c.IsDeleted);

                var shop = _shopRepo.GetFirstAsync(c => c.ShopId == request.ShopId && !c.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
                }

                if (shop == null)
                {
                    return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
                }

                var detail = _shopDetailRepo.GetFirstOrNullAsync(c => c.ProductId == request.ProductId
                                     && c.ShopId == request.ShopId
                                     && !c.IsDeleted);

                //If detail not exist, create new one
                if (detail == null)
                {
                    var shopDetail = _mapper.Map<ShopDetail>(request);
                    shopDetail.IsDeleted = false;

                    _shopDetailRepo.AddAsync(shopDetail);

                    return new BaseResponse("Create new detail successfully");
                }

                //If exist, add quantity for detail
                detail.Quantity += request.Quantity;

                _shopDetailRepo.UpdateAsync(detail);

                _memoryCacheService.RemoveCache($"Detail_{request.ShopId}");

                return new BaseResponse("Add quantity for detail because this detail has existed");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateDetail] Error: {ex.Message}");
                return new BaseResponse(StatusCodes.Status500InternalServerError, "Failed to create detail");
            }
        }
    }
}
