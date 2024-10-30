using AutoMapper;
using ShopManagement_Backend_Application.Models.Shop;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_Application.MappingProfiles
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<Shop, ShopResponse>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<ShopRequest, Shop>();
        }
    }
}
