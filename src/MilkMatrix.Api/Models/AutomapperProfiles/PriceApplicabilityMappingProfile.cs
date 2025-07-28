using AutoMapper;
using MilkMatrix.Api.Models.Request.PriceApplicability;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.PriceApplicability;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class PriceApplicabilityMappingProfile : Profile
    {
        public PriceApplicabilityMappingProfile() 
        {
            CreateMap<PriceAppUpdateRequestModel, PriceAppUpdateRequest>()
                        .ForMember(x => x.RateAppId, opt => opt.MapFrom(src => src.RateAppId))
                        .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                        .ForMember(x => x.RateCode, opt => opt.MapFrom(src => src.RateCode))
                        .ForMember(x => x.ModuleCode, opt => opt.MapFrom(src => src.ModuleCode))
                        .ForMember(x => x.ModuleName, opt => opt.MapFrom(src => src.ModuleName))
                        .ForMember(x => x.WithEffectDate, opt => opt.MapFrom(src => src.WithEffectDate))
                        .ForMember(x => x.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                        .ForMember(x => x.RateFor, opt => opt.MapFrom(src => src.RateFor))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<PriceAppInsertRequestModel, PriceAppInsertRequest>()
                        .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                        .ForMember(x => x.RateCode, opt => opt.MapFrom(src => src.RateCode))
                        .ForMember(x => x.ModuleCode, opt => opt.MapFrom(src => src.ModuleCode))
                        .ForMember(x => x.ModuleName, opt => opt.MapFrom(src => src.ModuleName))
                        .ForMember(x => x.WithEffectDate, opt => opt.MapFrom(src => src.WithEffectDate))
                        .ForMember(x => x.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                        .ForMember(x => x.RateFor, opt => opt.MapFrom(src => src.RateFor))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));

            CreateMap<RateForUpdateRequestModel, RateForUpdateRequest>()
                        .ForMember(x => x.RateForId, opt => opt.MapFrom(src => src.RateForId))
                        .ForMember(x => x.RateForCode, opt => opt.MapFrom(src => src.RateForCode))
                        .ForMember(x => x.RateForName, opt => opt.MapFrom(src => src.RateForName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<RateForInsertRequestModel, RateForInsertRequest>()
                        .ForMember(x => x.RateForCode, opt => opt.MapFrom(src => src.RateForCode))
                        .ForMember(x => x.RateForName, opt => opt.MapFrom(src => src.RateForName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
