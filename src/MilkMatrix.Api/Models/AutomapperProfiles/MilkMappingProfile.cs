using AutoMapper;
using MilkMatrix.Api.Models.Request.Milk;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Milk;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MilkMappingProfile : Profile
    {
        public MilkMappingProfile() 
        {
            CreateMap<MilkTypeUpdateRequestModel, MilkTypeUpdateRequest>()
                        .ForMember(x => x.MilkTypeId, opt => opt.MapFrom(src => src.MilkTypeId))
                        .ForMember(x => x.MilkTypeName, opt => opt.MapFrom(src => src.MilkTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<MilkTypeInsertRequestModel, MilkTypeInsertRequest>()
                        .ForMember(x => x.MilkTypeName, opt => opt.MapFrom(src => src.MilkTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
                    
            CreateMap<RateTypeUpdateRequestModel, RateTypeUpdateRequest>()
                        .ForMember(x => x.RateTypeId, opt => opt.MapFrom(src => src.RateTypeId))
                        .ForMember(x => x.RateTypeName, opt => opt.MapFrom(src => src.RateTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<RateTypeInsertRequestModel, RateTypeInsertRequest>()
                        .ForMember(x => x.RateTypeName, opt => opt.MapFrom(src => src.RateTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
