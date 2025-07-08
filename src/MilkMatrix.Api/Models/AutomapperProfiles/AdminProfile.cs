using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Approval.Level;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Api.Models.Request.Admin.Approval.Level;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;
using MilkMatrix.Api.Models.Request.Admin.User;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Infrastructure.Common.Utils;
using InsertDetails = MilkMatrix.Admin.Models.Admin.Requests.Approval.Details.Insert;
using InsertLevel = MilkMatrix.Admin.Models.Admin.Requests.Approval.Level.Insert;
using InsertDetailsModel = MilkMatrix.Api.Models.Request.Admin.Approval.Details.InsertModel;
using InsertLevelModel = MilkMatrix.Api.Models.Request.Admin.Approval.Level.InsertModel;

namespace MilkMatrix.Api.Models.AutomapperProfiles;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<LoginModel, LoginRequest>()
            .ForMember(x => x.HostName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.HostName]))
            .ForMember(x => x.PrivateIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PrivateIp]))
            .ForMember(x => x.PublicIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PublicIp]))
            .ForMember(x => x.BrowserName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.UserAgent]))
            .ForMember(x => x.IsLoginWithOtp, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.IsLoginWithOtp]));

        CreateMap<UserUpsertModel, UserUpdateRequest>()
                  .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.Roles))
                  .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Name))
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

        CreateMap<ForgotPasswordModel, ForgotPasswordRequest>();

        CreateMap<ResetPasswordModel, ResetPasswordRequest>();

        CreateMap<FinancialYearModel, FinancialYearRequest>()
            .ForMember(x => x.ActionType, opt => opt.MapFrom(src => src.Id != null && src.Id > 0 ? ReadActionType.Individual : ReadActionType.All));

        CreateMap<ChangePasswordModel, ChangePasswordRequest>();

        CreateMap<BusinessDataModel, BusinessDataRequest>()
            .ForMember(x => x.UserId, opt => opt.MapFrom((src, dest, destMember, context) => src.UserId != null && src.UserId > 0 ? src.UserId : context.Items[Constants.AutoMapper.LoginId]));

        CreateMap<BusinessInsertModel, BusinessInsertRequest>()
           .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<BusinessUpdateModel, BusinessUpdateRequest>()
           .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<ConfigurationInsertModel, ConfigurationInsertRequest>()
           .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<ConfigurationUpdateModel, ConfigurationUpdateRequest>()
           .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<UserProfileUpdateModel, UserProfileUpdate>()
                  .ForMember(x => x.UserId, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.LoginId]))
                  .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<SmtpSettingsInsertModel, SmtpSettingsInsert>()
                  .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<SmtpSettingsUpdateModel, SmtpSettingsUpdate>()
                  .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<BlockedMobileInsertModel, BlockedMobilesInsert>()
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<BlockedMobileUpdateModel, BlockedMobilesUpdate>()
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<SmsControlInsertModel, SmsControlInsert>()
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<SmsControlUpdateModel, SmsControlUpdate>()
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<InsertLevelModel, InsertLevel>()
         .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<UpdateModel, Update>()
           .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<InsertDetailsModel, InsertDetails>();
    }
}
