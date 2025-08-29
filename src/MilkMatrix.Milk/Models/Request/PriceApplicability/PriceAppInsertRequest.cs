using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.PriceApplicability
{
    public class PriceAppInsertRequest
    {
        public int? BusinessEntityId { get; set; }
        public string? RateCode { get; set; }
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public int? ShiftId { get; set; }
        public string? RateFor { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? Priority { get; set; }
        public string? cattleScope { get; set; }
        public string? applied_for { get; set; }
        public string? applied_shift_scope { get; set; }
        public string? Description { get; set; }
        public List<RateMappingTarget>? Targets { get; set; }
    }
}
