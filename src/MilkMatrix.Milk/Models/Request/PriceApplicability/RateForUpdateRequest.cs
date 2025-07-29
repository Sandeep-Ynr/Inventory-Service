using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.PriceApplicability
{
    public class RateForUpdateRequest
    {
        public string? RateForId { get; set; }
        public string? RateForCode { get; set; }
        public string? RateForName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
