using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Milk.Models.Request.PriceApplicability;

namespace MilkMatrix.Milk.Models.Response.PriceApplicability
{
    public class RateForInsertResponse
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? Priority { get; set; }
        public string? cattle_scope_Id { get; set; }
        public string? applied_for { get; set; }
        public string? applied_shift_scope { get; set; }
//        public string? Description { get; set; }
        public string? ScopeDetails { get; set; }
    }
}
