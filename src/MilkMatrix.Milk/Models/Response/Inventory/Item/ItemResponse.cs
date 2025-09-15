using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Inventory.Item
{
    public class ItemResponse
    {
        public long ItemId { get; set; }
        public int BusinessId { get; set; }
        public int ItemTypeId { get; set; }
        public int LifecycleStatusId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public int BaseUomId { get; set; }
        public bool IsPerishable { get; set; }
        public bool IsBatchTracked { get; set; }
        public bool IsSerialTracked { get; set; }
        public string? HsnSac { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        // Navigation properties (parsed JSON)
        public List<DairySpecResponse> DairySpecs { get; set; } = new();
        public List<ItemLocationResponse> Locations { get; set; } = new();
    }

    public class ItemResp
    {
        public int BusinessId { get; set; }
        public int ItemId { get; set; }
        public int ItemTypeId { get; set; }
        public int LifecycleStatusId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public int BaseUomId { get; set; }
        public bool IsPerishable { get; set; }
        public bool IsBatchTracked { get; set; }
        public bool IsSerialTracked { get; set; }
        public string? HsnSac { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? AvgRate { get; set; }
        public string? Barcode { get; set; }
        public string? Brand { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }

        // JSON stored as string
        public string? DairySpecs { get; set; }
        public string? Locations { get; set; }
    }

    public class DairySpecResponse
    {
        public int BusinessId { get; set; }
        public int DairyId { get; set; }
        public int ItemId { get; set; }
        public string MilkType { get; set; } = string.Empty;
        public string? Standardization { get; set; }
        public decimal FatPct { get; set; }
        public decimal SnfPct { get; set; }
        public decimal Density { get; set; }
        public decimal PackSizeValue { get; set; }
        public string PackSizeUom { get; set; } = string.Empty;
        public string PackType { get; set; } = string.Empty;
        public string StorageClass { get; set; } = string.Empty;
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }
        public int ShelfLife { get; set; }
        public string BatchPattern { get; set; } = string.Empty;
        public long ProductionShiftId { get; set; }
        public long SourceLevel { get; set; }
        public string SourceCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ItemLocationResponse
    {
        public int BusinessId { get; set; }
        public int ItemLocationId { get; set; }
        public int ItemId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public decimal MaxCapacity { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal ReorderLevel { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}
