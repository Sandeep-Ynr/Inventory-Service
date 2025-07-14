using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Route
{
    public class RouteResponse : CommonLists
    {
        public string? RegionalName { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public int VehicleCapacity { get; set; }
        public TimeSpan? MorningStartTime { get; set; }
        public TimeSpan? MorningEndTime { get; set; }
        public TimeSpan? EveningStartTime { get; set; }
        public TimeSpan? EveningEndTime { get; set; }
        public decimal TotalKm { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
        public bool is_status { get; set; }
    }
}
