using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.PriceApplicability
{
    public class PriceAppUpdateRequest
    {
        public int? RateAppId { get; set; }
        public int? BusinessEntityId { get; set; }
        public string? RateCode { get; set; }
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public int? ShiftId { get; set; }
        public string? RateFor { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
