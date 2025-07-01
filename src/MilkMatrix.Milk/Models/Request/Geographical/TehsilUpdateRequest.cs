using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class TehsilUpdateRequest
    {
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }
        public int? DistrictId { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
