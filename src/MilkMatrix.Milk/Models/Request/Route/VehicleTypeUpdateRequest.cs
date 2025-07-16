using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Route
{
    public class VehicleTypeUpdateRequest
    {
        public int VehicleID { get; set; }
        public string? VehicleType { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
        public int? ModifyBy { get; set; }
    }
}
