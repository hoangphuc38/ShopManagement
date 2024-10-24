using AutoMapper;
using Azure.Core;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public class ProductService
    {
        private readonly ShopManagementDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ShopManagementDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BaseResponse GetAll()
        {
            var productList = _context.Products.Where(c => c.IsDeleted == false).ToList();
            var responseList = new List<ProductResponse>();

            foreach (var product in productList)
            {
                var response = _mapper.Map<ProductResponse>(product);
                responseList.Add(response);
            }

            return new BaseResponse(responseList);
        }

        public BaseResponse GetDetailProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            var response = _mapper.Map<ProductResponse>(product);

            return new BaseResponse(response);
        }

        public BaseResponse UpdateProduct(int id, ProductRequest request)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            product.ProductName = request.ProductName;
            product.Price = request.Price;

            _context.Products.Update(product);
            _context.SaveChanges();

            return new BaseResponse("Update product successfully");
        }

        public BaseResponse DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new BaseResponse(StatusCodes.Status404NotFound, "Product not found");
            }

            product.IsDeleted = true;
            _context.Products.Update(product);

            var detailList = _context.ShopDetails.Where(c => c.ProductId == id).ToList();
            foreach (var detail in detailList)
            {
                detail.IsDeleted = true;
                _context.ShopDetails.Update(detail);
            }

            _context.SaveChanges();

            return new BaseResponse("Delete product successfully");
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.IsDeleted = false;

            _context.Products.Add(product);
            _context.SaveChanges();

            return new BaseResponse("Create product successfully");
        }
    }
}
