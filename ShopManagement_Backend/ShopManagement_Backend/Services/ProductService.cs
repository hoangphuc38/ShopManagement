using Azure.Core;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public class ProductService
    {
        public ShopManagementDbContext _context;

        public ProductService(ShopManagementDbContext context)
        {
            _context = context;
        }

        public BaseResponse GetAll()
        {
            var productList = _context.Products.Where(c => c.IsDeleted == false).ToList();
            var responseList = new List<ProductResponse>();

            foreach (var product in productList)
            {
                ProductResponse response = new ProductResponse
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                };

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

            ProductResponse response = new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
            };

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
            _context.SaveChanges();

            return new BaseResponse("Delete product successfully");
        }

        public BaseResponse CreateProduct(ProductRequest request)
        {
            Product product = new Product
            {
                ProductName = request.ProductName,
                Price = request.Price,
                IsDeleted = false,
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return new BaseResponse("Create product successfully");
        }
    }
}
