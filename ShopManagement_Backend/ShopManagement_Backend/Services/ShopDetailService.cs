using AutoMapper;
using Azure.Core;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public class ShopDetailService
    {
        private readonly ShopManagementDbContext _context;
        private readonly IMapper _mapper;

        public ShopDetailService(ShopManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseResponse GetAllOfShop(int id)
        {
            var productList = _context.ShopDetails
                                      .Where(c => c.ShopId == id && c.IsDeleted == false)
                                      .ToList();
            var responseList = new List<ShopDetailResponse>();

            foreach (var product in productList)
            {
                var productRes = _context.Products
                                         .Where(c => c.ProductId == product.ProductId && c.IsDeleted == false)
                                         .FirstOrDefault();

                if (productRes == null)
                {
                    continue;
                }

                var response = _mapper.Map<ShopDetailResponse>(product);

                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }

        public BaseResponse UpdateDetail(ShopDetailRequest request)
        {
            var product = _context.Products
                                  .Where(c => c.ProductId == request.ProductId && c.IsDeleted == false)
                                  .FirstOrDefault();

            var shop = _context.Shops
                               .Where(c => c.ShopId == request.ShopId && c.IsDeleted == false)
                               .FirstOrDefault();
            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            var detail = _context.ShopDetails
                                 .Where(c => c.ProductId == request.ProductId
                                 && c.ShopId == request.ShopId
                                 && c.IsDeleted == false)
                                 .FirstOrDefault();

            if (detail == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
            }

            detail.Quantity = request.Quantity;
            
            _context.ShopDetails.Update(detail);
            _context.SaveChanges();

            return new BaseResponse("Update detail successfully");
        }

        public BaseResponse DeleteDetail(int shopID, int productID)
        {
            var detail = _context.ShopDetails
                                 .Where(c => c.ProductId == productID
                                 && c.ShopId == shopID
                                 && c.IsDeleted == false)
                                 .FirstOrDefault();

            if (detail == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Not exist this detail");
            }

            detail.IsDeleted = true;

            _context.ShopDetails.Update(detail);
            _context.SaveChanges();

            return new BaseResponse("Delete detail successfully");
        }

        public BaseResponse CreateDetail(ShopDetailRequest request)
        {
            var product = _context.Products
                                  .Where(c => c.ProductId == request.ProductId && c.IsDeleted == false)
                                  .FirstOrDefault();

            var shop = _context.Shops
                               .Where(c => c.ShopId == request.ShopId && c.IsDeleted == false)
                               .FirstOrDefault();
            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            if (shop == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Shop not found");
            }

            var detail = _context.ShopDetails
                                 .Where(c => c.ProductId == request.ProductId
                                 && c.ShopId == request.ShopId
                                 && c.IsDeleted == false)
                                 .FirstOrDefault();

            //If detail not exist, create new one
            if (detail == null)
            {
                var shopDetail = _mapper.Map<ShopDetail>(request);
                shopDetail.IsDeleted = false;

                _context.ShopDetails.Add(shopDetail);
                _context.SaveChanges();

                return new BaseResponse("Create new detail successfully");
            }

            //If exist, add quantity for detail
            detail.Quantity += request.Quantity;

            _context.ShopDetails.Update(detail);
            _context.SaveChanges();

            return new BaseResponse("Add quantity for detail because this detail has existed");
        }
    }
}
