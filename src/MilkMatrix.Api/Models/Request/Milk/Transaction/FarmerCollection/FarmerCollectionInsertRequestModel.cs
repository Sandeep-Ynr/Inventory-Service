namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerCollection
{
    public class FarmerCollectionInsertRequestModel
    {
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public int BusinessId { get; set; }
        public long MccId { get; set; }
        public long BmcId { get; set; }
        public int RouteID { get; set; }
        public int MPPID { get; set; }
        public long FarmerId { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? LR { get; set; }
        public decimal WeightLiter { get; set; }
        public string? Type { get; set; }
        public decimal? Rtpl { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? SampleId { get; set; }
        public string? BatchNo { get; set; }
        public string? FarmerName { get; set; }
        public string? Mobile { get; set; }
        public long? CompanyCode { get; set; }
        public string? IMEI_No { get; set; }
        public string Shift { get; set; } = string.Empty;
        public string Status { get; set; } = "APPROVED";
        public long StatusId { get; set; }
        public long CId { get; set; }
        public DateTime? CDate { get; set; }
        public long? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool? IsStatus { get; set; }
       
    }
}
