using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Logistics.Route
{
    public class RouteRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? RouteID { get; set; }
        public bool? IsActive { get; set; }
    }
}