using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Milk
{
    public class MeasurementUnitUpdateRequest
    {
        public string? MeasurementUnitId { get; set; }
        public string? MeasurementUnitCode { get; set; }
        public string? MeasurementUnitName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
