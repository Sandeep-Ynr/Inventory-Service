namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollStgInsertRequestModel
    {
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public string Shift { get; set; } = string.Empty;
        public string? BatchNo { get; set; }
        public long MppId { get; set; }
        public long BmcID { get; set; }
        public string InsertMode { get; set; } = string.Empty;   // IMP / MANUAL
        public string Status { get; set; } = string.Empty;       // PENDING / APPROVED
        public long? CompanyCode { get; set; }
        public string? ImeiNo { get; set; }
        public bool IsValidated { get; set; }
        public bool IsProcess { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string? BusinessId { get; set; }
        public List<FarmerCollectionStagingDetailModel> Details { get; set; } = new();

    }
    public class FarmerCollectionStagingDetailModel
    {
        public long? FarmerId { get; set; }
        public string? Mobile { get; set; }
        public string? FarmerCode { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? LR { get; set; }
        public decimal? WeightLiter { get; set; }
        public string? Type { get; set; }          // Cow / Buffalo
        public decimal? Rptl { get; set; } // RTPL
        public decimal? TotalAmount { get; set; }
        public int? SampleId { get; set; }
        public int? Can { get; set; }
        public long? ReferenceId { get; set; }
    }
}
