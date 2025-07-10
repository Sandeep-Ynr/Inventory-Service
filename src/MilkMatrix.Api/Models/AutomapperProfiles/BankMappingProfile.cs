using AutoMapper;
using Azure.Core;
using MilkMatrix.Api.Models.Request.Bank.Bank;
using MilkMatrix.Api.Models.Request.Bank.BankRegional;
using MilkMatrix.Api.Models.Request.Bank.BankType;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Bank;
namespace MilkMatrix.Api.Models.AutomapperProfiles;
public class BankMappingProfile : Profile
{
    public BankMappingProfile()
    {

        CreateMap<BankRegInsertReqModel, BankRegInsertRequest>()
            .ForMember(x => x.RegionalCode, opt => opt.MapFrom(src => src.RegionalCode))
            .ForMember(x => x.BankID, opt => opt.MapFrom(src => src.BankID))
            .ForMember(x => x.RegionalBankName, opt => opt.MapFrom(src => src.RegionalBankName))
            .ForMember(x => x.RegionalBankShortName, opt => opt.MapFrom(src => src.RegionalBankShortName))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        CreateMap<BankRegUpdateReqModel, BankRegUpdateRequest>()
            .ForMember(x => x.RegionalID, opt => opt.MapFrom(src => src.RegionalID))
            .ForMember(x => x.RegionalCode, opt => opt.MapFrom(src => src.RegionalCode))
            .ForMember(x => x.RegionalBankName, opt => opt.MapFrom(src => src.RegionalBankName))
            .ForMember(x => x.RegionalBankShortName, opt => opt.MapFrom(src => src.RegionalBankShortName))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));
        CreateMap<BankTypeInsertRequestModel, BankTypeInsertRequest>()
            .ForMember(dest => dest.BankTypeName, opt => opt.MapFrom(src => src.BankTypeName))
            .ForMember(dest => dest.BankTypeDescription, opt => opt.MapFrom(src => src.BankTypeDescription))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["CreatedBy"]));
        CreateMap<BankTypeUpdateRequestModel, BankTypeUpdateRequest>()
            .ForMember(dest => dest.BankTypeId, opt => opt.MapFrom(src => src.BankTypeId))
            .ForMember(dest => dest.BankTypeName, opt => opt.MapFrom(src => src.BankTypeName))
            .ForMember(dest => dest.BankTypeDescription, opt => opt.MapFrom(src => src.BankTypeDescription))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));

        CreateMap<BankInsertRequestModel, BankInsertRequest>()
            .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
            .ForMember(dest => dest.BankShortName, opt => opt.MapFrom(src => src.BankShortName))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.StateId))
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.AccountNoLength, opt => opt.MapFrom(src => src.AccountNoLength))
            .ForMember(dest => dest.IsAccountValidationEnabled, opt => opt.MapFrom(src => src.IsAccountValidationEnabled))
            .ForMember(dest => dest.IsNationalized, opt => opt.MapFrom(src => src.IsNationalized))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus)) // updated name
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

        CreateMap<BankUpdateRequestModel, BankUpdateRequest>()
            .ForMember(dest => dest.BankTypeID, opt => opt.MapFrom(src => src.BankTypeID))
            .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
            .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.BankName))
            .ForMember(dest => dest.BankShortName, opt => opt.MapFrom(src => src.BankShortName))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.StateId, opt => opt.MapFrom(src => src.StateId))
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.AccountNoLength, opt => opt.MapFrom(src => src.AccountNoLength))
            .ForMember(dest => dest.IsAccountValidationEnabled, opt => opt.MapFrom(src => src.IsAccountValidationEnabled))
            .ForMember(dest => dest.IsNationalized, opt => opt.MapFrom(src => src.IsNationalized))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus)) // updated name
            .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ModifiedBy"]));
    }
}
