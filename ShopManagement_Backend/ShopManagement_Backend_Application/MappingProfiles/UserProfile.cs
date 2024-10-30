using AutoMapper;
using ShopManagement_Backend_Application.Models.User;
using ShopManagement_Backend_Core.Entities;

namespace ShopManagement_Backend_Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserRequest, User>();
        }
    }
}
