using AutoMapper;
using ShopManagement_Backend_Application.Models.Product;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();
        }
    }
}
