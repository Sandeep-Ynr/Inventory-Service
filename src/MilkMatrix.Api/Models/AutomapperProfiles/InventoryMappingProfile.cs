using AutoMapper;
using MilkMatrix.Api.Models.Request.Inventory.Item;
using MilkMatrix.Api.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Request.Inventory.Item;
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

            // Mapping for Insert
            CreateMap<ItemInsertRequestModel, ItemInsertRequest>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.ItemTypeId, opt => opt.MapFrom(src => src.ItemTypeId))
                .ForMember(dest => dest.LifecycleStatusId, opt => opt.MapFrom(src => src.LifecycleStatusId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.BaseUomId, opt => opt.MapFrom(src => src.BaseUomId))
                .ForMember(dest => dest.Is_Perishable, opt => opt.MapFrom(src => src.Is_Perishable))
                .ForMember(dest => dest.Is_Batch_Tracked, opt => opt.MapFrom(src => src.Is_Batch_Tracked))
                .ForMember(dest => dest.Is_Serial_Tracked, opt => opt.MapFrom(src => src.Is_Serial_Tracked))
                .ForMember(dest => dest.Hsn_Sac, opt => opt.MapFrom(src => src.Hsn_Sac))
                .ForMember(dest => dest.Mrp, opt => opt.MapFrom(src => src.Mrp))
                .ForMember(dest => dest.Purchase_Rate, opt => opt.MapFrom(src => src.Purchase_Rate))
                .ForMember(dest => dest.Sale_Rate, opt => opt.MapFrom(src => src.Sale_Rate))
                .ForMember(dest => dest.Avg_Rate, opt => opt.MapFrom(src => src.Avg_Rate))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Is_Active, opt => opt.MapFrom(src => src.Is_Active))
                //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)) // Usually handled internally
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => DateTime.Now));

            // Mapping for Update
            CreateMap<ItemUpdateRequestModel, ItemUpdateRequest>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                .ForMember(dest => dest.ItemTypeId, opt => opt.MapFrom(src => src.ItemTypeId))
                .ForMember(dest => dest.LifecycleStatusId, opt => opt.MapFrom(src => src.LifecycleStatusId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.SubCategoryId))
                .ForMember(dest => dest.ItemCode, opt => opt.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
                .ForMember(dest => dest.BaseUomId, opt => opt.MapFrom(src => src.BaseUomId))
                .ForMember(dest => dest.Is_Perishable, opt => opt.MapFrom(src => src.Is_Perishable))
                .ForMember(dest => dest.Is_Batch_Tracked, opt => opt.MapFrom(src => src.Is_Batch_Tracked))
                .ForMember(dest => dest.Is_Serial_Tracked, opt => opt.MapFrom(src => src.Is_Serial_Tracked))
                .ForMember(dest => dest.Hsn_Sac, opt => opt.MapFrom(src => src.Hsn_Sac))
                .ForMember(dest => dest.Mrp, opt => opt.MapFrom(src => src.Mrp))
                .ForMember(dest => dest.Purchase_Rate, opt => opt.MapFrom(src => src.Purchase_Rate))
                .ForMember(dest => dest.Sale_Rate, opt => opt.MapFrom(src => src.Sale_Rate))
                .ForMember(dest => dest.Avg_Rate, opt => opt.MapFrom(src => src.Avg_Rate))
                .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Is_Active, opt => opt.MapFrom(src => src.Is_Active))
                //.ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted)) // Usually handled internally
                .ForMember(dest => dest.ModifyBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]))
                .ForMember(dest => dest.ModifyOn, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
