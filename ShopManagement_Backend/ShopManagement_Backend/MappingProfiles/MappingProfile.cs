﻿using AutoMapper;
using ShopManagement_Backend.Models;
using ShopManagement_Backend.Requests;
using ShopManagement_Backend.Responses;

namespace ShopManagement_Backend.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();

            CreateMap<Shop, ShopResponse>();
            CreateMap<ShopRequest, Shop>();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserRequest, User>();

            CreateMap<ShopDetail, ShopDetailResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            CreateMap<ShopDetailRequest, ShopDetail>();
        }
    }
}