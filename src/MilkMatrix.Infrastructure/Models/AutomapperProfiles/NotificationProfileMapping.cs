using AutoMapper;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Notifications.Models.Enums;
using MilkMatrix.Notifications.Models.OTP.Request;
using MilkMatrix.Notifications.Models.OTP.Response;

namespace MilkMatrix.Infrastructure.Models.AutomapperProfiles;

public class NotificationProfileMapping : Profile
{
    public NotificationProfileMapping()
    {
        CreateMap<NotificationRequest, OTPRequest>()
            .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
            .ForMember(dest => dest.EmailId, opt => opt.MapFrom(src => src.EmailId))
            .ForMember(dest => dest.TemplateType, opt => opt.MapFrom(src => (TemplateType)src.TemplateType))
            .ForMember(dest => dest.OTPType, opt => opt.MapFrom(src => (OTPType)src.OTPType))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
        CreateMap<NotificationResponse, OTPResponse>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

        CreateMap<OTPResponse, NotificationResponse>()
         .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
         .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));
    }
}
