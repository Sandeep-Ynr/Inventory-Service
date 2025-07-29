using AutoMapper;
using MilkMatrix.Api.Models.Request.Price;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Price;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MilkPriceMappingProfile : Profile
    {
        public MilkPriceMappingProfile() 
        {
            // Update mapping for insert request
            CreateMap<MilkPriceInsertRequestModel, MilkPriceInsertRequest>()
                .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                .ForMember(x => x.WithEffectDate, opt => opt.MapFrom(src => src.WithEffectDate))
                .ForMember(x => x.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                .ForMember(x => x.MilkTypeId, opt => opt.MapFrom(src => src.MilkTypeId))
                .ForMember(x => x.RateTypeId, opt => opt.MapFrom(src => src.RateTypeId))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.RateGenType, opt => opt.MapFrom(src => src.RateGenType))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]))
                .ForMember(x => x.PriceDetails, opt => opt.MapFrom(src => src.PriceDetails)); // <- Fix here

            // Fix: Add correct mapping for child list (PriceDetails)
            CreateMap<MilkPriceDetailModel, MilkPriceDetailRequest>();

            // Existing update mapping
            CreateMap<MilkPriceUpdateRequestModel, MilkPriceUpdateRequest>()
                .ForMember(x => x.RateCode, opt => opt.MapFrom(src => src.RateCode))
                .ForMember(x => x.BusinessEntityId, opt => opt.MapFrom(src => src.BusinessEntityId))
                .ForMember(x => x.WithEffectDate, opt => opt.MapFrom(src => src.WithEffectDate))
                .ForMember(x => x.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                .ForMember(x => x.MilkTypeId, opt => opt.MapFrom(src => src.MilkTypeId))
                .ForMember(x => x.RateTypeId, opt => opt.MapFrom(src => src.RateTypeId))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(x => x.RateGenType, opt => opt.MapFrom(src => src.RateGenType))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));


        }
    }
}
