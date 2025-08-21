
using System;

namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollStgUpdateRequestModel
    {
        public decimal CollecionID { get; set; }
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public long? FarmerId { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? LR { get; set; }
        public decimal? WeightLiter { get; set; }
        public string? Type { get; set; }
        public decimal? Rtpl { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? SampleId { get; set; }
        public string? BatchNo { get; set; }
        public string? FarmerName { get; set; }
        public string? Mobile { get; set; }
        public string? InsertMode { get; set; }
        public string? Status { get; set; }
        public string? Shift { get; set; }
        public long MppID { get; set; }
        public long BmcID { get; set; }
        public long? RefranceId { get; set; }
        public int? Can { get; set; }
        public bool IsValidated { get; set; }
        public bool IsProcess { get; set; }
        public long CId { get; set; }
        public DateTime CDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public long? CompanyCode { get; set; }
        public string? IMEINo { get; set; }
        public bool? IsStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }
}
