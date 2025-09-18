using MilkMatrix.Milk.Models.Request.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Inventory.Item;

namespace MilkMatrix.Api.Models.Request.Inventory.Item
{
    public class ItemInsertRequestModel
    {
        public int? BusinessId { get; set; }
        public int? ItemTypeId { get; set; }
        public int? LifecycleStatusId { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public int? BaseUomId { get; set; }
        public int? brand_id { get; set; }
        public bool? Is_Perishable { get; set; }
        public bool? Is_Batch_Tracked { get; set; }
        public bool? Is_Serial_Tracked { get; set; }
        public string? Hsn_Sac { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Purchase_Rate { get; set; }
        public decimal? Sale_Rate { get; set; }
        public decimal? Avg_Rate { get; set; }
        public string? Barcode { get; set; }
        public string? Notes { get; set; }
        public bool? Is_Active { get; set; }
        public List<DairySpecInsertRequest> DairySpecs { get; set; } = new();
        public List<ItemLocationInsertRequest> Locations { get; set; } = new();
    }

    public class DairySpecInsertRequestModel
    {
        public int? BusinessId { get; set; }
        public string? MilkType { get; set; }
        public string? Standardization { get; set; }
        public decimal? FatPct { get; set; }        // DECIMAL(4,2)
        public decimal? SnfPct { get; set; }        // DECIMAL(4,2)
        public decimal? Density { get; set; }       // DECIMAL(5,3)
        public decimal? PackSizeValue { get; set; } // DECIMAL(10,3)
        public string? PackSizeUom { get; set; }
        public string? PackType { get; set; }
        public string? StorageClass { get; set; }
        public decimal? TempMin { get; set; }       // DECIMAL(5,2)
        public decimal? TempMax { get; set; }       // DECIMAL(5,2)
        public int? ShelfLife { get; set; }
        public string? BatchPattern { get; set; }
        public long? ProductionShiftId { get; set; } // BIGINT
        public long? SourceLevel { get; set; }       // BIGINT
        public string? SourceCode { get; set; }
        public bool? Is_Active { get; set; }
    }

    public class ItemLocationInsertRequestModel
    {
        public int? BusinessId { get; set; }
        public long? LocationId { get; set; }   // BIGINT
        public long? bin_id { get; set; }   // BIGINT
        public decimal? MaxCapacity { get; set; }       // DECIMAL(18,3)
        public decimal? OpeningBalance { get; set; }    // DECIMAL(18,3)
        public decimal? CurrentBalance { get; set; }    // DECIMAL(18,3)
        public decimal? ReorderLevel { get; set; }      // DECIMAL(18,3)
        public decimal? MinQty { get; set; }            // DECIMAL(18,3)
        public decimal? MaxQty { get; set; }            // DECIMAL(18,3)
        public bool? Is_Active { get; set; }
    }
}
