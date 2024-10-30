using AutoMapper;
using ShopManagement_Backend_Application.Models.ShopDetail;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_Application.MappingProfiles
{
    public class ShopDetailProfile : Profile
    {
        public ShopDetailProfile()
        {
            CreateMap<ShopDetail, ShopDetailResponse>()
                .ForPath(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            CreateMap<ShopDetailRequest, ShopDetail>();
        }
    }
}
