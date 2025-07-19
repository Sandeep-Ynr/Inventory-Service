using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Logistics.Vehicle
{
    public class VehicleRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string? VehicleID { get; set; }
        public int? TransporterID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
