using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerCollection
{
    public class FarmerCollectionResponse
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public DateTime DumpDate { get; set; }
        public string? DumpTime { get; set; }
        public int FarmerId { get; set; }
        public string? FarmerName { get; set; }
        public decimal Fat { get; set; }
        public decimal Snf { get; set; }
        public decimal LR { get; set; }
        public decimal WeightLiter { get; set; }
        public string? Type { get; set; }
        public decimal Rtpl { get; set; }
        public decimal TotalAmount { get; set; }
        public int SampleId { get; set; }
        public string? BatchNo { get; set; }
        public string? Mobile { get; set; }
        public int CompanyCode { get; set; }
        public string? IMEINo { get; set; }
        public int BmcId { get; set; }
        public int MccId { get; set; }
        public string? BmcCode { get; set; }
        public string? MccCode { get; set; }
        public string? Shift { get; set; }
        public string? Status { get; set; }
        public int StatusId { get; set; }
        public int CId { get; set; }
        public DateTime CDate { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public string? BusinessName { get; set; }
    }
}
