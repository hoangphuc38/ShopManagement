using AutoMapper;
using ShopManagement_Backend_Application.Models.Notification;
using ShopManagement_Backend_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement_Backend_Application.MappingProfiles
{
    public class NotificationRecepientProfile : Profile
    {
        public NotificationRecepientProfile()
        {
            CreateMap<NotificationRecepient, NotificationResponse>()
                .ForPath(dest => dest.NotificationContent, opt => opt.MapFrom(src => src.Notification));
        }
    }
}
