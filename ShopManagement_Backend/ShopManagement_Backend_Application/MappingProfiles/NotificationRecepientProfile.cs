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
            .ForMember(dest => dest.NotificationRecepientID, opt => opt.MapFrom(src => src.NotificationRecepientId))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead ?? false))  // Default IsRead to false if null
            .ForPath(dest => dest.NotificationContent.Title, opt => opt.MapFrom(src => src.Notification.Title))
            .ForPath(dest => dest.NotificationContent.Content, opt => opt.MapFrom(src => src.Notification.Content))
            .ForPath(dest => dest.NotificationContent.SenderInfo, opt => opt.MapFrom(src => src.Notification.SenderInfo));
        }
    }
}
