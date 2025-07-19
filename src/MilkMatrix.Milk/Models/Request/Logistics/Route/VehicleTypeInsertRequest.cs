using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Logistics.Route
{
    public class VehicleTypeInsertRequest
    {
        public string VehicleType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool IsStatus { get; set; } = true;
        public int CreatedBy { get; set; }
    }
}
