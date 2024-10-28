using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.Services
{
    public interface IProductService
    {
        BaseResponse GetAll();

        BaseResponse GetDetailProduct(int id);

        BaseResponse UpdateProduct(int id, ProductRequest request);

        BaseResponse DeleteProduct(int id);

        BaseResponse CreateProduct(ProductRequest request);

    }
}
