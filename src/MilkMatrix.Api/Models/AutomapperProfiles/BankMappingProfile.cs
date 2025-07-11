using AutoMapper;
using Azure.Core;
using MilkMatrix.Api.Models.Request.Bank.Bank;
using MilkMatrix.Api.Models.Request.Bank.BankRegional;
using MilkMatrix.Api.Models.Request.Bank.BankType;
using MilkMatrix.Api.Models.Request.Bank.Branch;
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


    CreateMap<BranchInsertRequestModel, BranchInsertRequest>()
           .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
           .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
           .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
           .ForMember(dest => dest.LocalBranchName, opt => opt.MapFrom(src => src.LocalBranchName))
           .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
           .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
           .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
           .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
           .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
           .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
           .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
           .ForMember(dest => dest.AddressHindi, opt => opt.MapFrom(src => src.AddressHindi))
           .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
           .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
           .ForMember(dest => dest.ContactNo, opt => opt.MapFrom(src => src.ContactNo))
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
           .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));


            CreateMap<BranchUpdateRequestModel, BranchUpdateRequest>()
            .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
            .ForMember(dest => dest.BranchCode, opt => opt.MapFrom(src => src.BranchCode))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.LocalBranchName, opt => opt.MapFrom(src => src.LocalBranchName))
            .ForMember(dest => dest.IFSC, opt => opt.MapFrom(src => src.IFSC))
            .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
            .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
            .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
            .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
            .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.AddressHindi, opt => opt.MapFrom(src => src.AddressHindi))
            .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
            .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
            .ForMember(dest => dest.ContactNo, opt => opt.MapFrom(src => src.ContactNo))
            .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

    }
}
