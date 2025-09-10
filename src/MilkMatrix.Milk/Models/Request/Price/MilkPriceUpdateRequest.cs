using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Price
{
    public class MilkPriceUpdateRequest
    {
        public string? RateCode { get; set; }
        public int? BusinessEntityId { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public string? ShiftId { get; set; }
        public string? MilkTypeId { get; set; }
        public string? RateTypeId { get; set; }
        public string? Description { get; set; }
        public string? RateGenType { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }

        public List<MilkPriceDetailRequest>? PriceDetails { get; set; }
    }
}
