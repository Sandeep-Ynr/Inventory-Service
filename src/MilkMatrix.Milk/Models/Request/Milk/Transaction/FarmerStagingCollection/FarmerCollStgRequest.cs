using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollStgRequest
    {
        public decimal RowId { get; set; }
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public int BusinessEntityId { get; set; }
        public string Mppcode { get; set; }
        public string BatchNo { get; set; }
        public long? ReferenceId { get; set; }
        public long? FarmerId { get; set; }
        public string? FName { get; set; }
        public string Shift { get; set; }
        public string Type { get; set; }
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
        public string InsertMode { get; set; }
        public string? IMEI_No { get; set; }
        public bool IsValidated { get; set; }
        public bool IsProcess { get; set; }
        public DateTime? ProcessDate { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
