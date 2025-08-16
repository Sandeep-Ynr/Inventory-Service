using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.BlockedMobiles;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.CommonStatus;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Configurations;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Email;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Sms;
using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Api.Models.Request.Admin.Approval.Level;
using MilkMatrix.Api.Models.Request.Admin.Business;
using MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;
using MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings.CommonStatus;
using MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Api.Models.Request.Admin.Rejection;
using MilkMatrix.Api.Models.Request.Admin.User;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request.Approval.Level;
using MilkMatrix.Core.Entities.Request.Rejection;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance;
using InsertDetails = MilkMatrix.Core.Entities.Request.Approval.Details.Insert;
using InsertDetailsModel = MilkMatrix.Api.Models.Request.Admin.Approval.Details.InsertModel;
using InsertLevel = MilkMatrix.Core.Entities.Request.Approval.Level.Insert;
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
                  .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]))
                  .ForMember(x => x.IsMFA, opt => opt.MapFrom(src => src.IsMFA.HasValue && src.IsMFA == YesOrNo.Yes ? 'Y' : 'N'));

        CreateMap<UserUpsertModel, UserInsertRequest>()
                  .ForMember(x => x.RoleId, opt => opt.MapFrom(src => src.Roles))
                  .ForMember(x => x.Username, opt => opt.MapFrom(src => src.Name))
                  .ForMember(x => x.EmailId, opt => opt.MapFrom(src => src.Email))
                  .ForMember(x => x.MobileNumber, opt => opt.MapFrom(src => src.Mobile))
                  .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessIds))
                  .ForMember(x => x.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? src.Password.EncodeSHA512() : null))
                  .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]))
                  .ForMember(x => x.IsMFA, opt => opt.MapFrom(src => src.IsMFA.HasValue && src.IsMFA == YesOrNo.Yes ? 'Y' : 'N'));

        CreateMap<ForgotPasswordModel, ForgotPasswordRequest>();

        CreateMap<ResetPasswordModel, ResetPasswordRequest>();

        CreateMap<FinancialYearModel, FinancialYearRequest>()
            .ForMember(x => x.ActionType, opt => opt.MapFrom(src => src.Id != null && src.Id > 0 ? ReadActionType.Individual : ReadActionType.All));

        CreateMap<ChangePasswordModel, ChangePasswordRequest>()
            .ForMember(x => x.LoggedInUser, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.LoginId]))
             .ForMember(x => x.UserId, opt => opt.MapFrom(src => src.EmailId));

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

        CreateMap<RejectionModel, InsertRejection>()
           .ForMember(x => x.RejectedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<StatusInsertModel, CommonStatusInsert>()
            .ForMember(x=>x.StatusName , opt=> opt.MapFrom(src=> src.Name))
                        .ForMember(x => x.StatusType, opt => opt.MapFrom(src => src.Type))
           .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        CreateMap<StatusUpdateModel, CommonStatusUpdate>()
           .ForMember(x => x.StatusName, opt => opt.MapFrom(src => src.Name))
           .ForMember(x => x.StatusType, opt => opt.MapFrom(src => src.Type))
           .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<LogoutModel, LogoutRequest>();

        // Insert mapping
        CreateMap<SequanceInsertRequestModel, SequenceInsertRequest>()
            .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.HeadName))
            .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => src.Prefix))
            .ForMember(dest => dest.StartValue, opt => opt.MapFrom(src => src.StartValue))
            .ForMember(dest => dest.StopValue, opt => opt.MapFrom(src => src.StopValue))
            .ForMember(dest => dest.IncrementValue, opt => opt.MapFrom(src => src.IncrementValue))
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));




        // Update mapping
        CreateMap<SequanceUpdateRequestModel, SequenceUpdateRequest>()
            .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.HeadName))
            .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => src.Prefix))
            .ForMember(dest => dest.StartValue, opt => opt.MapFrom(src => src.StartValue))
            .ForMember(dest => dest.StopValue, opt => opt.MapFrom(src => src.StopValue))
            .ForMember(dest => dest.IncrementValue, opt => opt.MapFrom(src => src.IncrementValue))
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));
    }
}
