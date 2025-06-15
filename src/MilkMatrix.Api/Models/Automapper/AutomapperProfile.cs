using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Api.Models.Request.Admin;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Infrastructure.Common.Utils;

namespace DetaServices.Models.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<LoginModel, LoginRequest>()
                .ForMember(x => x.HostName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.HostName]))
                .ForMember(x => x.PrivateIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PrivateIp]))
                .ForMember(x => x.PublicIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PublicIp]))
                .ForMember(x => x.BrowserName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.UserAgent]))
                .ForMember(x => x.IsLoginWithOtp, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.IsLoginWithOtp]));

            CreateMap<UserUpsertModel, UserUpdateRequest>()
                      .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.Roles))
                      .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Name))
                      .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.Email))
                      .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessIds))
                      .ForMember(x => x.MobileNumber, opt => opt.MapFrom(src => src.Mobile))
                      .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.Status))
                      .ForMember(x => x.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? src.Password.EncodeSHA512() : null))
                      .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<UserUpsertModel, UserInsertRequest>()
                      .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.Roles))
                      .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Name))
                      .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.Email))
                      .ForMember(x => x.MobileNumber, opt => opt.MapFrom(src => src.Mobile))
                      .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessIds))
                      .ForMember(x => x.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? src.Password.EncodeSHA512() : null))
                      .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
