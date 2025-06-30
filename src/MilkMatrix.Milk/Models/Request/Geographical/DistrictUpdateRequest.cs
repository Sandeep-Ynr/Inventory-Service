using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class DistrictUpdateRequest
    {
        public int DistrictId { get; set; }
        public int? StateId { get; set; }
        public string? DistrictName { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
