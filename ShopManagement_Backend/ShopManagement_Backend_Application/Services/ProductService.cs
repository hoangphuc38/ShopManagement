﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ShopManagement_Backend_Application.Hubs;
using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Application.Services.Interfaces;
using ShopManagement_Backend_Core.Constants;
using ShopManagement_Backend_Core.Entities;
using ShopManagement_Backend_DataAccess.Repositories.Interfaces;
using System.Reflection;
using System.Resources;

namespace ShopManagement_Backend_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;
        private readonly IShopDetailRepository _shopDetailRepo;
        private readonly INotificationRepository _notiRepo;
        private readonly INotificationRecepientRepository _notiRecepRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<ProductService> _logger;
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IHubContext<StockHub> _stockHub;
        private readonly ResourceManager _resource;

        public ProductService(
            IMapper mapper,
            IProductRepository productRepo,
            IShopDetailRepository shopDetailRepo,
            INotificationRepository notiRepo,
            INotificationRecepientRepository notiRecepRepo,
            IUserRepository userRepo,
            ILogger<ProductService> logger,
            IMemoryCacheService memoryCacheService,
            IHubContext<StockHub> stockHub)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _shopDetailRepo = shopDetailRepo;
            _notiRepo = notiRepo;
            _notiRecepRepo = notiRecepRepo;
            _userRepo = userRepo;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
            _stockHub = stockHub;
            _resource = new ResourceManager(
                "ShopManagement_Backend_Application.Resources.Messages.ProductMessages",
                Assembly.GetExecutingAssembly());
        }

        public async Task<BaseResponse> GetProductsWithPagination(ProductPaginationRequest request)
        {
            try
            {
                _logger.LogInformation($"[GetAllProduct] Start to get all products.");
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
                    filter = $" AND ProductName LIKE '%{request.SearchText}%' ";
                }

                string sortOrder = request.Sort == SortType.Ascending ? SortType.AscendingSort : SortType.DescendingSort;

                string sort = $" ORDER BY {request.Column} {sortOrder} ";

                var productList = _productRepo.GetProductsWithPagination(
                    request.PageIndex, request.PageSize, sort, filter, out totalRecords);
                
                if (totalRecords == 0)
                {
                    return new BaseResponse(new PaginationResponse(request.PageIndex, request.PageSize, productList));
                }

                int totalPages = totalRecords / request.PageSize + 1;

                var productMapperList = _mapper.Map<List<ProductResponse>>(productList);

                var response = new PaginationResponse
                {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalOfNumberRecord = totalRecords,
                    TotalOfPages = totalPages,
                    Results = productMapperList
                };

                return new BaseResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetAllProduct] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetListFailed") ?? "");
            }
            
        }

        public async Task<BaseResponse> GetDetailProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[GetDetailProduct] Start to get detail product by ProductId: {id}.");

                var response = _memoryCacheService.GetCacheData($"Product_{id}");

                if (response == null)
                {
                    var product = await _productRepo.GetProductById(id);

                    if (product == null)
                    {
                        return new BaseResponse(
                            StatusCodes.Status404NotFound,
                            _resource.GetString("NotFoundProduct") ?? "");
                    }

                    var productMapper = _mapper.Map<ProductResponse>(product);

                    response = new BaseResponse(productMapper);

                    _memoryCacheService.SetCache($"Product_{id}", response);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GetDetailProduct] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("GetDetailFailed") ?? "");
            }
        }

        public async Task<BaseResponse> UpdateProduct(int id, ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[UpdateProduct] Start to update product with ProductId: {id}, " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");

                var product = await _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundProduct") ?? "");
                }

                product.ProductName = request.ProductName;
                product.Price = request.Price;
                product.ImageUrl = request.ImageUrl;

                await _productRepo.UpdateAsync(product);

                _memoryCacheService.RemoveCache($"Product_{id}");

                return new BaseResponse(_resource.GetString("UpdateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UpdateProduct] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("UpdateFailed") ?? "");
            }
            
        }

        public async Task<BaseResponse> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"[DeleteProduct] Start to delete product by ProductId: {id}.");
                var product = await _productRepo.GetFirstAsync(t => t.ProductId == id && !t.IsDeleted);

                if (product == null)
                {
                    return new BaseResponse(
                        StatusCodes.Status404NotFound,
                        _resource.GetString("NotFoundProduct") ?? "");
                }

                product.IsDeleted = true;
                await _productRepo.UpdateAsync(product);

                var detailList = await _shopDetailRepo.GetAllAsync(c => c.ProductId == id && !c.IsDeleted);
                foreach (var detail in detailList)
                {
                    detail.IsDeleted = true;
                    await _shopDetailRepo.UpdateAsync(detail);
                }

                _memoryCacheService.RemoveCache($"Product_{id}");

                return new BaseResponse(_resource.GetString("DeleteSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DeleteProduct] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("DeleteFailed") ?? "");
            }
        }

        public async Task<BaseResponse> CreateProduct(ProductRequest request)
        {
            try
            {
                _logger.LogInformation($"[CreateProduct] Start to create product. " +
                    $"ProductName: {request.ProductName}, Price: {request.Price}");
                var product = _mapper.Map<Product>(request);
                product.IsDeleted = false;

                await _productRepo.AddAsync(product);

                var notification = new Notification
                {
                    Title = "Sản phẩm mới",
                    Content = $"Đã thêm sản phẩm mới: {request.ProductName} với giá {request.Price}",
                    SentDate = DateTime.Now,
                    SenderInfo = "Admin"
                };

                await _notiRepo.AddAsync(notification);

                var userList = await _userRepo.GetAllAsync(t => !t.IsDeleted);

                foreach (var user in userList)
                {
                    var notificationRecep = new NotificationRecepient
                    {
                        UserId = user.Id,
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                    };

                    await _notiRecepRepo.AddAsync(notificationRecep);   
                }

                await _stockHub.Clients.All.SendAsync(
                    "NewProductNoti", request.ProductName, request.Price);

                return new BaseResponse(_resource.GetString("CreateSuccess") ?? "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CreateProduct] Error: {ex.Message}");
                return new BaseResponse(
                    StatusCodes.Status500InternalServerError,
                    _resource.GetString("CreateFailed") ?? "");
            }          
        }
    }
}
