using ShopManagement_Backend_Application.Models;
using ShopManagement_Backend_Application.Models.Product;

namespace ShopManagement_Backend_Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<BaseResponse> GetAll();

        Task<BaseResponse> GetDetailProduct(int id);

        Task<BaseResponse> UpdateProduct(int id, ProductRequest request);

        Task<BaseResponse> DeleteProduct(int id);

        Task<BaseResponse> CreateProduct(ProductRequest request);
    }
}
