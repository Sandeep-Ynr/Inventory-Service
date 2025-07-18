using AutoMapper;
using MilkMatrix.Api.Models.Request.Animal;
using MilkMatrix.Milk.Models;
using MilkMatrix.Milk.Models.Request.Animal;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class AnimalMappingProfile : Profile
    {
        public AnimalMappingProfile() 
        {
            CreateMap<AnimalTypeUpdateRequestModel, AnimalTypeUpdateRequest>()
                        .ForMember(x => x.AnimalTypeId, opt => opt.MapFrom(src => src.AnimalTypeId))
                        .ForMember(x => x.AnimalTypeCode, opt => opt.MapFrom(src => src.AnimalTypeCode))
                        .ForMember(x => x.AnimalTypeName, opt => opt.MapFrom(src => src.AnimalTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<AnimalTypeInsertRequestModel, AnimalTypeInsertRequest>()
                        .ForMember(x => x.AnimalTypeCode, opt => opt.MapFrom(src => src.AnimalTypeCode))
                        .ForMember(x => x.AnimalTypeName, opt => opt.MapFrom(src => src.AnimalTypeName))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
