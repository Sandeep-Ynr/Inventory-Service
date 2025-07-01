using AutoMapper;
using MilkMatrix.Api.Models.Request.Geographical.District;
using MilkMatrix.Api.Models.Request.Geographical.Hamlet;
using MilkMatrix.Api.Models.Request.Geographical.State;
using MilkMatrix.Api.Models.Request.Geographical.Tehsil;
using MilkMatrix.Api.Models.Request.Geographical.Village;
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
        CreateMap<HamletRequestModel, HamletRequest>();
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
        CreateMap<VillageRequestModel, VillageRequest>();
    }
}
