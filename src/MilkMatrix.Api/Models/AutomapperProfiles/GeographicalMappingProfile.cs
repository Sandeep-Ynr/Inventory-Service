using AutoMapper;
using Azure.Core;
using MilkMatrix.Api.Models.Request.Geographical.BankRegional;
using MilkMatrix.Api.Models.Request.Geographical.District;
using MilkMatrix.Api.Models.Request.Geographical.Hamlet;
using MilkMatrix.Api.Models.Request.Geographical.State;
using MilkMatrix.Api.Models.Request.Geographical.Tehsil;
using MilkMatrix.Api.Models.Request.Geographical.Village;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Geographical;

namespace MilkMatrix.Api.Models.AutomapperProfiles;

public class GeographicalMappingProfile : Profile
{
    public GeographicalMappingProfile()
    {
        CreateMap<DistrictUpdateRequestModel, DistrictUpdateRequest>()
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.DistrictName, opt => opt.MapFrom(src => src.DistrictName))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));
        CreateMap<DistrictInsertRequestModel, DistrictInsertRequest>()
                        .ForMember(x => x.DistrictName, opt => opt.MapFrom(src => src.DistrictName))
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        CreateMap<DistrictRequestModel, DistrictRequest>();
        
        CreateMap<StateUpsertModel, StateUpdateRequest>()
                        .ForMember(x => x.StateId, opt => opt.MapFrom(src => src.StateId))
                        .ForMember(x => x.StateName, opt => opt.MapFrom(src => src.StateName))
                        .ForMember(x => x.CountryId, opt => opt.MapFrom(src => src.CountryId))
                        .ForMember(x => x.AreaCode, opt => opt.MapFrom(src => src.AreaCode))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));
        CreateMap<StateRequestModel, StateRequest>();
        CreateMap<StateUpsertModel, StateInsertRequest>()
                        .ForMember(x => x.StateName, opt => opt.MapFrom(src => src.StateName))
                        .ForMember(x => x.CountryId, opt => opt.MapFrom(src => src.CountryId))
                        .ForMember(x => x.AreaCode, opt => opt.MapFrom(src => src.AreaCode))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        CreateMap<TehsilUpdateRequestModel, TehsilUpdateRequest>()
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.TehsilName, opt => opt.MapFrom(src => src.TehsilName))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));
        CreateMap<TehsilInsertRequestModel, TehsilInsertRequest>()
                        .ForMember(x => x.TehsilName, opt => opt.MapFrom(src => src.TehsilName))
                        .ForMember(x => x.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        CreateMap<TehsilRequestModel, TehsilRequest>();


        CreateMap<VillageInsertRequestModel, VillageInsertRequest>()
                        .ForMember(x => x.VillageName, opt => opt.MapFrom(src => src.VillageName))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        
        CreateMap<VillageUpdateRequestModel, VillageUpdateRequest>()
                         .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
                         .ForMember(x => x.VillageName, opt => opt.MapFrom(src => src.VillageName))
                        .ForMember(x => x.TehsilId, opt => opt.MapFrom(src => src.TehsilId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

         CreateMap<VillageRequestModel, VillageRequest>();

         CreateMap<HamletInsertRequestModel, HamletInsertRequest>()
            .ForMember(x => x.HamletName, opt => opt.MapFrom(src => src.HamletName))
            .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
            .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

         CreateMap<HamletUpdateRequestModel, HamletUpdateRequest>()
            .ForMember(x => x.HamletId, opt => opt.MapFrom(src => src.HamletId))
            .ForMember(x => x.HamletName, opt => opt.MapFrom(src => src.HamletName))
            .ForMember(x => x.VillageId, opt => opt.MapFrom(src => src.VillageId))
            .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

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
    }
}
