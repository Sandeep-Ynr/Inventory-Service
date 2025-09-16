namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollStgInsertRequestModel
    {
        
        public List<FarmerCollStgInsertRequestListModel>? Import { get; set; }

    }
    public class FarmerCollStgInsertRequestListModel
    {
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public int BusinessEntityId { get; set; }
        public string? Mppcode { get; set; }
        public string? BatchNo { get; set; }
        public long? ReferenceId { get; set; }
        public long? FarmerId { get; set; }
        public string? FName { get; set; }
        public long? CId { get; set; }
        public DateTime CDate { get; set; }
        public string? Shift { get; set; }
        public string? Type { get; set; }
        public decimal WeightLiter { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? LR { get; set; }
        public int? Can { get; set; }
        public decimal? Rtpl { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? SampleId { get; set; }
        public string? IsQltyAuto { get; set; }
        public string? IsQtyAuto { get; set; }
        public string? InsertMode { get; set; }
        public string? IMEI_No { get; set; }
        public bool IsValidated { get; set; }
        public bool IsProcess { get; set; }
        public DateTime? ProcessDate { get; set; }
        public bool? IsStatus { get; set; }
    }
}
