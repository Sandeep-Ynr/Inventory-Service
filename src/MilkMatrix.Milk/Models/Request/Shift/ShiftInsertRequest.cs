using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Shift
{
    public class ShiftInsertRequest
    {
        public string? ShiftCode { get; set; }
        public string? ShiftName { get; set; }
        public string? ShiftTime { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
    }
}
