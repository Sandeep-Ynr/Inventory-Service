using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Logistics.Route
{
    public class VehicleTypeInsertRequestModel
    {
        public string VehicleType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
