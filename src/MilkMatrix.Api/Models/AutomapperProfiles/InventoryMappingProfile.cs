using AutoMapper;
using MilkMatrix.Api.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Request.Inventory.ItemCategory;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            // Mapping for Insert
            CreateMap<ItemCatgInsertRequestModel, ItemCatgInsertRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));

            // Mapping for Update
            CreateMap<ItemCatgUpdateRequestModel, ItemCatgUpdateRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]))
                .ForMember(dest => dest.ModifyOn, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
