using AutoMapper;
using MilkMatrix.Milk.Models;
using MilkMatrix.Api.Models.Request.Shift;
using MilkMatrix.Milk.Models.Request.Shift;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class ShiftMappingProfile : Profile
    {
        public ShiftMappingProfile() 
        {
            CreateMap<ShiftUpdateRequestModel, ShiftUpdateRequest>()
                        .ForMember(x => x.ShiftId, opt => opt.MapFrom(src => src.ShiftId))
                        .ForMember(x => x.ShiftCode, opt => opt.MapFrom(src => src.ShiftCode))
                        .ForMember(x => x.ShiftName, opt => opt.MapFrom(src => src.ShiftName))
                        .ForMember(x => x.ShiftTime, opt => opt.MapFrom(src => src.ShiftTime))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<ShiftInsertRequestModel, ShiftInsertRequest>()
                        .ForMember(x => x.ShiftCode, opt => opt.MapFrom(src => src.ShiftCode))
                        .ForMember(x => x.ShiftName, opt => opt.MapFrom(src => src.ShiftName))
                        .ForMember(x => x.ShiftTime, opt => opt.MapFrom(src => src.ShiftTime))
                        .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
