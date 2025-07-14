using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Route
{
    public class RouteInsertRequest
    {
        public string Name { get; set; } = string.Empty;
        public string RouteCode { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string? RegionalName { get; set; }
        public int VehicleID { get; set; }
        public int VehicleCapacity { get; set; }
        public TimeSpan? MorningStartTime { get; set; }
        public TimeSpan? MorningEndTime { get; set; }
        public TimeSpan? EveningStartTime { get; set; }
        public TimeSpan? EveningEndTime { get; set; }
        public decimal TotalKm { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
    }
}
