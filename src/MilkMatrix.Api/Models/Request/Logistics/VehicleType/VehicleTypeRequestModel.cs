using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Logistics.Vehicle
{
    public class VehicleTypeRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? VehicleID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
