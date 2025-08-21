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
using MilkMatrix.Api.Models.Request.Admin.GlobleSetting.ConfigSettings;
using MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance;
using MilkMatrix.Api.Models.Request.Admin.Rejection;
using MilkMatrix.Api.Models.Request.Admin.User;
using MilkMatrix.Api.Models.Request.Login;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Request.Approval.Level;
using MilkMatrix.Core.Entities.Request.Rejection;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.ConfigSettings;
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

        // Insert mapping for Trans
        CreateMap<SequanceTransInsertRequestModel, SequenceTransInsertRequest>()
            .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.HeadName))
            .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => src.Prefix))
            .ForMember(dest => dest.StartValue, opt => opt.MapFrom(src => src.StartValue))
            .ForMember(dest => dest.StopValue, opt => opt.MapFrom(src => src.StopValue))
            .ForMember(dest => dest.IncrementValue, opt => opt.MapFrom(src => src.IncrementValue))
            .ForMember(dest => dest.fy_year, opt => opt.MapFrom(src => src.fy_year))
            .ForMember(dest => dest.delimiter, opt => opt.MapFrom(src => src.delimiter))
            .ForMember(dest => dest.suffix, opt => opt.MapFrom(src => src.suffix))
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

        // Update mapping  for Trans
        CreateMap<SequanceTransUpdateRequestModel, SequenceTransUpdateRequest >()
            .ForMember(dest => dest.HeadName, opt => opt.MapFrom(src => src.HeadName))
            .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => src.Prefix))
            .ForMember(dest => dest.StartValue, opt => opt.MapFrom(src => src.StartValue))
            .ForMember(dest => dest.StopValue, opt => opt.MapFrom(src => src.StopValue))
            .ForMember(dest => dest.IncrementValue, opt => opt.MapFrom(src => src.IncrementValue))
            .ForMember(dest => dest.fy_year, opt => opt.MapFrom(src => src.fy_year))
            .ForMember(dest => dest.delimiter, opt => opt.MapFrom(src => src.delimiter))
            .ForMember(dest => dest.suffix, opt => opt.MapFrom(src => src.suffix))
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

        CreateMap<ConfigSettingInsertRequestModel, ConfigSettingInsertRequest>()
            .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
            .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => src.UnitType))
            .ForMember(dest => dest.GenlCanPerLit, opt => opt.MapFrom(src => src.GenlCanPerLit))
            .ForMember(dest => dest.GenlCanWarThr, opt => opt.MapFrom(src => src.GenlCanWarThr))
            .ForMember(dest => dest.GenlLtrToKgConFac, opt => opt.MapFrom(src => src.GenlLtrToKgConFac))
            .ForMember(dest => dest.GenlWeiRouMod, opt => opt.MapFrom(src => src.GenlWeiRouMod))
            .ForMember(dest => dest.GenlQuaRouMod, opt => opt.MapFrom(src => src.GenlQuaRouMod))
            .ForMember(dest => dest.GenlMemQuaMod, opt => opt.MapFrom(src => src.GenlMemQuaMod))
            .ForMember(dest => dest.GenlBmcQuaMod, opt => opt.MapFrom(src => src.GenlBmcQuaMod))
            .ForMember(dest => dest.GenlUseDefSnf, opt => opt.MapFrom(src => src.GenlUseDefSnf))
            .ForMember(dest => dest.GenlDefSnfVal, opt => opt.MapFrom(src => src.GenlDefSnfVal))
            .ForMember(dest => dest.GenlZerRatAll, opt => opt.MapFrom(src => src.GenlZerRatAll))
            .ForMember(dest => dest.GenlEnaShiWisEntDat, opt => opt.MapFrom(src => src.GenlEnaShiWisEntDat))
            .ForMember(dest => dest.QcdlColMinFat, opt => opt.MapFrom(src => src.QcdlColMinFat))
            .ForMember(dest => dest.QcdlColMaxFat, opt => opt.MapFrom(src => src.QcdlColMaxFat))
            .ForMember(dest => dest.QcdlColMinSnf, opt => opt.MapFrom(src => src.QcdlColMinSnf))
            .ForMember(dest => dest.QcdlColMaxSnf, opt => opt.MapFrom(src => src.QcdlColMaxSnf))
            .ForMember(dest => dest.QcdlDisQtyMod, opt => opt.MapFrom(src => src.QcdlDisQtyMod))
            .ForMember(dest => dest.QcdlDisLtrKg, opt => opt.MapFrom(src => src.QcdlDisLtrKg))
            .ForMember(dest => dest.QcdlDisMinFat, opt => opt.MapFrom(src => src.QcdlDisMinFat))
            .ForMember(dest => dest.QcdlDisMaxFat, opt => opt.MapFrom(src => src.QcdlDisMaxFat))
            .ForMember(dest => dest.QcdlDisMinSnf, opt => opt.MapFrom(src => src.QcdlDisMinSnf))
            .ForMember(dest => dest.QcdlDisMaxSnf, opt => opt.MapFrom(src => src.QcdlDisMaxSnf))
            .ForMember(dest => dest.DvalMilTypDva, opt => opt.MapFrom(src => src.DvalMilTypDva))
            .ForMember(dest => dest.DvalDefFat, opt => opt.MapFrom(src => src.DvalDefFat))
            .ForMember(dest => dest.DvalDefSnf, opt => opt.MapFrom(src => src.DvalDefSnf))
            .ForMember(dest => dest.WsetIsActWse, opt => opt.MapFrom(src => src.WsetIsActWse))
            .ForMember(dest => dest.WsetWeiRouBy, opt => opt.MapFrom(src => src.WsetWeiRouBy))
            .ForMember(dest => dest.WsetCanAveWei, opt => opt.MapFrom(src => src.WsetCanAveWei))
            .ForMember(dest => dest.LsetTraSiz, opt => opt.MapFrom(src => src.LsetTraSiz))
            .ForMember(dest => dest.LsetSnfRou, opt => opt.MapFrom(src => src.LsetSnfRou))
            .ForMember(dest => dest.LsetSnfRouBy, opt => opt.MapFrom(src => src.LsetSnfRouBy))
            .ForMember(dest => dest.LsetSnfAftDec, opt => opt.MapFrom(src => src.LsetSnfAftDec))
            .ForMember(dest => dest.LsetSnfFor, opt => opt.MapFrom(src => src.LsetSnfFor))
            .ForMember(dest => dest.LsetSnfCon, opt => opt.MapFrom(src => src.LsetSnfCon))
            .ForMember(dest => dest.LsetFatRou, opt => opt.MapFrom(src => src.LsetFatRou))
            .ForMember(dest => dest.LsetFatRouBy, opt => opt.MapFrom(src => src.LsetFatRouBy))
            .ForMember(dest => dest.LsetFatAftDec, opt => opt.MapFrom(src => src.LsetFatAftDec))
            .ForMember(dest => dest.LsetLrRou, opt => opt.MapFrom(src => src.LsetLrRou))
            .ForMember(dest => dest.LsetLrRouBy, opt => opt.MapFrom(src => src.LsetLrRouBy))
            .ForMember(dest => dest.LsetLrAftDec, opt => opt.MapFrom(src => src.LsetLrAftDec))
            .ForMember(dest => dest.QlmtMilTypQlm, opt => opt.MapFrom(src => src.QlmtMilTypQlm))
            .ForMember(dest => dest.QlmtMinFat, opt => opt.MapFrom(src => src.QlmtMinFat))
            .ForMember(dest => dest.QlmtMaxFat, opt => opt.MapFrom(src => src.QlmtMaxFat))
            .ForMember(dest => dest.QlmtMinSnf, opt => opt.MapFrom(src => src.QlmtMinSnf))
            .ForMember(dest => dest.QlmtMaxSnf, opt => opt.MapFrom(src => src.QlmtMaxSnf))
            .ForMember(dest => dest.QlmtMinClr, opt => opt.MapFrom(src => src.QlmtMinClr))
            .ForMember(dest => dest.QlmtMaxClr, opt => opt.MapFrom(src => src.QlmtMaxClr))
            .ForMember(dest => dest.AdltTesNam, opt => opt.MapFrom(src => src.AdltTesNam))
            .ForMember(dest => dest.AdltIsEnaAdl, opt => opt.MapFrom(src => src.AdltIsEnaAdl))
            .ForMember(dest => dest.FuncAllFarCodEdi, opt => opt.MapFrom(src => src.FuncAllFarCodEdi))
            .ForMember(dest => dest.FuncValRatRanOnImp, opt => opt.MapFrom(src => src.FuncValRatRanOnImp))
            .ForMember(dest => dest.FuncLoaAllRouOnDoc, opt => opt.MapFrom(src => src.FuncLoaAllRouOnDoc))
            .ForMember(dest => dest.FuncAllDupFarCol, opt => opt.MapFrom(src => src.FuncAllDupFarCol))
            .ForMember(dest => dest.FuncEnaPayCyc, opt => opt.MapFrom(src => src.FuncEnaPayCyc))
            .ForMember(dest => dest.UnitBmcColMod, opt => opt.MapFrom(src => src.UnitBmcColMod))
            .ForMember(dest => dest.UnitMppColMod, opt => opt.MapFrom(src => src.UnitMppColMod))
            .ForMember(dest => dest.UnitLitToKgCon, opt => opt.MapFrom(src => src.UnitLitToKgCon))
            .ForMember(dest => dest.UnitKgToLitCon, opt => opt.MapFrom(src => src.UnitKgToLitCon))
            .ForMember(dest => dest.EntrMulSamTyp, opt => opt.MapFrom(src => src.EntrMulSamTyp))
            .ForMember(dest => dest.EntrMulDifTyp, opt => opt.MapFrom(src => src.EntrMulDifTyp))
            .ForMember(dest => dest.InptInpFat, opt => opt.MapFrom(src => src.InptInpFat))
            .ForMember(dest => dest.InptInpSnf, opt => opt.MapFrom(src => src.InptInpSnf))
            .ForMember(dest => dest.InptInpClr, opt => opt.MapFrom(src => src.InptInpClr))
            .ForMember(dest => dest.InptInpPro, opt => opt.MapFrom(src => src.InptInpPro))
            .ForMember(dest => dest.InptInpLac, opt => opt.MapFrom(src => src.InptInpLac))
            .ForMember(dest => dest.InptInpWat, opt => opt.MapFrom(src => src.InptInpWat))
            .ForMember(dest => dest.VariVarFat, opt => opt.MapFrom(src => src.VariVarFat))
            .ForMember(dest => dest.VariBloFat, opt => opt.MapFrom(src => src.VariBloFat))
            .ForMember(dest => dest.VariVarSnf, opt => opt.MapFrom(src => src.VariVarSnf))
            .ForMember(dest => dest.VariBloSnf, opt => opt.MapFrom(src => src.VariBloSnf))
            .ForMember(dest => dest.VariVarQty, opt => opt.MapFrom(src => src.VariVarQty))
            .ForMember(dest => dest.VariBloQty, opt => opt.MapFrom(src => src.VariBloQty))
            .ForMember(dest => dest.MachMacBasOn, opt => opt.MapFrom(src => src.MachMacBasOn))
            .ForMember(dest => dest.MachMacShi, opt => opt.MapFrom(src => src.MachMacShi))
            .ForMember(dest => dest.MachMacNo, opt => opt.MapFrom(src => src.MachMacNo))
            .ForMember(dest => dest.DsplDisBasOn, opt => opt.MapFrom(src => src.DsplDisBasOn))
            .ForMember(dest => dest.DsplDisShi, opt => opt.MapFrom(src => src.DsplDisShi))
            .ForMember(dest => dest.DsplDisNo, opt => opt.MapFrom(src => src.DsplDisNo))
            .ForMember(dest => dest.OthrIsAutMer, opt => opt.MapFrom(src => src.OthrIsAutMer))
            .ForMember(dest => dest.OthrRatChaApp, opt => opt.MapFrom(src => src.OthrRatChaApp))
             .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

        CreateMap<ConfigSettingUpdateRequestModel, ConfigSettingUpdateRequest>()
           .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
           .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitId))
           .ForMember(dest => dest.UnitType, opt => opt.MapFrom(src => src.UnitType))
           .ForMember(dest => dest.GenlCanPerLit, opt => opt.MapFrom(src => src.GenlCanPerLit))
           .ForMember(dest => dest.GenlCanWarThr, opt => opt.MapFrom(src => src.GenlCanWarThr))
           .ForMember(dest => dest.GenlLtrToKgConFac, opt => opt.MapFrom(src => src.GenlLtrToKgConFac))
           .ForMember(dest => dest.GenlWeiRouMod, opt => opt.MapFrom(src => src.GenlWeiRouMod))
           .ForMember(dest => dest.GenlQuaRouMod, opt => opt.MapFrom(src => src.GenlQuaRouMod))
           .ForMember(dest => dest.GenlMemQuaMod, opt => opt.MapFrom(src => src.GenlMemQuaMod))
           .ForMember(dest => dest.GenlBmcQuaMod, opt => opt.MapFrom(src => src.GenlBmcQuaMod))
           .ForMember(dest => dest.GenlUseDefSnf, opt => opt.MapFrom(src => src.GenlUseDefSnf))
           .ForMember(dest => dest.GenlDefSnfVal, opt => opt.MapFrom(src => src.GenlDefSnfVal))
           .ForMember(dest => dest.GenlZerRatAll, opt => opt.MapFrom(src => src.GenlZerRatAll))
           .ForMember(dest => dest.GenlEnaShiWisEntDat, opt => opt.MapFrom(src => src.GenlEnaShiWisEntDat))
           .ForMember(dest => dest.QcdlColMinFat, opt => opt.MapFrom(src => src.QcdlColMinFat))
           .ForMember(dest => dest.QcdlColMaxFat, opt => opt.MapFrom(src => src.QcdlColMaxFat))
           .ForMember(dest => dest.QcdlColMinSnf, opt => opt.MapFrom(src => src.QcdlColMinSnf))
           .ForMember(dest => dest.QcdlColMaxSnf, opt => opt.MapFrom(src => src.QcdlColMaxSnf))
           .ForMember(dest => dest.QcdlDisQtyMod, opt => opt.MapFrom(src => src.QcdlDisQtyMod))
           .ForMember(dest => dest.QcdlDisLtrKg, opt => opt.MapFrom(src => src.QcdlDisLtrKg))
           .ForMember(dest => dest.QcdlDisMinFat, opt => opt.MapFrom(src => src.QcdlDisMinFat))
           .ForMember(dest => dest.QcdlDisMaxFat, opt => opt.MapFrom(src => src.QcdlDisMaxFat))
           .ForMember(dest => dest.QcdlDisMinSnf, opt => opt.MapFrom(src => src.QcdlDisMinSnf))
           .ForMember(dest => dest.QcdlDisMaxSnf, opt => opt.MapFrom(src => src.QcdlDisMaxSnf))
           .ForMember(dest => dest.DvalMilTypDva, opt => opt.MapFrom(src => src.DvalMilTypDva))
           .ForMember(dest => dest.DvalDefFat, opt => opt.MapFrom(src => src.DvalDefFat))
           .ForMember(dest => dest.DvalDefSnf, opt => opt.MapFrom(src => src.DvalDefSnf))
           .ForMember(dest => dest.WsetIsActWse, opt => opt.MapFrom(src => src.WsetIsActWse))
           .ForMember(dest => dest.WsetWeiRouBy, opt => opt.MapFrom(src => src.WsetWeiRouBy))
           .ForMember(dest => dest.WsetCanAveWei, opt => opt.MapFrom(src => src.WsetCanAveWei))
           .ForMember(dest => dest.LsetTraSiz, opt => opt.MapFrom(src => src.LsetTraSiz))
           .ForMember(dest => dest.LsetSnfRou, opt => opt.MapFrom(src => src.LsetSnfRou))
           .ForMember(dest => dest.LsetSnfRouBy, opt => opt.MapFrom(src => src.LsetSnfRouBy))
           .ForMember(dest => dest.LsetSnfAftDec, opt => opt.MapFrom(src => src.LsetSnfAftDec))
           .ForMember(dest => dest.LsetSnfFor, opt => opt.MapFrom(src => src.LsetSnfFor))
           .ForMember(dest => dest.LsetSnfCon, opt => opt.MapFrom(src => src.LsetSnfCon))
           .ForMember(dest => dest.LsetFatRou, opt => opt.MapFrom(src => src.LsetFatRou))
           .ForMember(dest => dest.LsetFatRouBy, opt => opt.MapFrom(src => src.LsetFatRouBy))
           .ForMember(dest => dest.LsetFatAftDec, opt => opt.MapFrom(src => src.LsetFatAftDec))
           .ForMember(dest => dest.LsetLrRou, opt => opt.MapFrom(src => src.LsetLrRou))
           .ForMember(dest => dest.LsetLrRouBy, opt => opt.MapFrom(src => src.LsetLrRouBy))
           .ForMember(dest => dest.LsetLrAftDec, opt => opt.MapFrom(src => src.LsetLrAftDec))
           .ForMember(dest => dest.QlmtMilTypQlm, opt => opt.MapFrom(src => src.QlmtMilTypQlm))
           .ForMember(dest => dest.QlmtMinFat, opt => opt.MapFrom(src => src.QlmtMinFat))
           .ForMember(dest => dest.QlmtMaxFat, opt => opt.MapFrom(src => src.QlmtMaxFat))
           .ForMember(dest => dest.QlmtMinSnf, opt => opt.MapFrom(src => src.QlmtMinSnf))
           .ForMember(dest => dest.QlmtMaxSnf, opt => opt.MapFrom(src => src.QlmtMaxSnf))
           .ForMember(dest => dest.QlmtMinClr, opt => opt.MapFrom(src => src.QlmtMinClr))
           .ForMember(dest => dest.QlmtMaxClr, opt => opt.MapFrom(src => src.QlmtMaxClr))
           .ForMember(dest => dest.AdltTesNam, opt => opt.MapFrom(src => src.AdltTesNam))
           .ForMember(dest => dest.AdltIsEnaAdl, opt => opt.MapFrom(src => src.AdltIsEnaAdl))
           .ForMember(dest => dest.FuncAllFarCodEdi, opt => opt.MapFrom(src => src.FuncAllFarCodEdi))
           .ForMember(dest => dest.FuncValRatRanOnImp, opt => opt.MapFrom(src => src.FuncValRatRanOnImp))
           .ForMember(dest => dest.FuncLoaAllRouOnDoc, opt => opt.MapFrom(src => src.FuncLoaAllRouOnDoc))
           .ForMember(dest => dest.FuncAllDupFarCol, opt => opt.MapFrom(src => src.FuncAllDupFarCol))
           .ForMember(dest => dest.FuncEnaPayCyc, opt => opt.MapFrom(src => src.FuncEnaPayCyc))
           .ForMember(dest => dest.UnitBmcColMod, opt => opt.MapFrom(src => src.UnitBmcColMod))
           .ForMember(dest => dest.UnitMppColMod, opt => opt.MapFrom(src => src.UnitMppColMod))
           .ForMember(dest => dest.UnitLitToKgCon, opt => opt.MapFrom(src => src.UnitLitToKgCon))
           .ForMember(dest => dest.UnitKgToLitCon, opt => opt.MapFrom(src => src.UnitKgToLitCon))
           .ForMember(dest => dest.EntrMulSamTyp, opt => opt.MapFrom(src => src.EntrMulSamTyp))
           .ForMember(dest => dest.EntrMulDifTyp, opt => opt.MapFrom(src => src.EntrMulDifTyp))
           .ForMember(dest => dest.InptInpFat, opt => opt.MapFrom(src => src.InptInpFat))
           .ForMember(dest => dest.InptInpSnf, opt => opt.MapFrom(src => src.InptInpSnf))
           .ForMember(dest => dest.InptInpClr, opt => opt.MapFrom(src => src.InptInpClr))
           .ForMember(dest => dest.InptInpPro, opt => opt.MapFrom(src => src.InptInpPro))
           .ForMember(dest => dest.InptInpLac, opt => opt.MapFrom(src => src.InptInpLac))
           .ForMember(dest => dest.InptInpWat, opt => opt.MapFrom(src => src.InptInpWat))
           .ForMember(dest => dest.VariVarFat, opt => opt.MapFrom(src => src.VariVarFat))
           .ForMember(dest => dest.VariBloFat, opt => opt.MapFrom(src => src.VariBloFat))
           .ForMember(dest => dest.VariVarSnf, opt => opt.MapFrom(src => src.VariVarSnf))
           .ForMember(dest => dest.VariBloSnf, opt => opt.MapFrom(src => src.VariBloSnf))
           .ForMember(dest => dest.VariVarQty, opt => opt.MapFrom(src => src.VariVarQty))
           .ForMember(dest => dest.VariBloQty, opt => opt.MapFrom(src => src.VariBloQty))
           .ForMember(dest => dest.MachMacBasOn, opt => opt.MapFrom(src => src.MachMacBasOn))
           .ForMember(dest => dest.MachMacShi, opt => opt.MapFrom(src => src.MachMacShi))
           .ForMember(dest => dest.MachMacNo, opt => opt.MapFrom(src => src.MachMacNo))
           .ForMember(dest => dest.DsplDisBasOn, opt => opt.MapFrom(src => src.DsplDisBasOn))
           .ForMember(dest => dest.DsplDisShi, opt => opt.MapFrom(src => src.DsplDisShi))
           .ForMember(dest => dest.DsplDisNo, opt => opt.MapFrom(src => src.DsplDisNo))
           .ForMember(dest => dest.OthrIsAutMer, opt => opt.MapFrom(src => src.OthrIsAutMer))
           .ForMember(dest => dest.OthrRatChaApp, opt => opt.MapFrom(src => src.OthrRatChaApp))
           .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));





    }
}
