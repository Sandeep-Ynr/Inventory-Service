using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection;

namespace MilkMatrix.Milk.Models.Response.Price
{
    public class MilkPriceInsertResponse
    {
        public string? Id { get; set; }
        public int? BusinessEntityId { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public string? ShiftId { get; set; }
        public string? MilkTypeId { get; set; }
        public string? RateTypeId { get; set; }
        public string? Description { get; set; }
        public string? RateGenType { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public List<MilkPriceDetailResponse>? PriceDetails { get; set; }

     
    }
    public class MilkPriceInsertResp
    {
        public string? Id { get; set; }
        public int? BusinessEntityId { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public string? ShiftId { get; set; }
        public string? MilkTypeId { get; set; }
        public string? RateTypeId { get; set; }
        public string? Description { get; set; }
        public string? RateGenType { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public string? PriceDetails { get; set; }
    }

    public class MilkPriceDetailResponse
    {
        public decimal fat { get; set; }

        public decimal snf { get; set; }
        public decimal rate { get; set; }
        
    }
}
