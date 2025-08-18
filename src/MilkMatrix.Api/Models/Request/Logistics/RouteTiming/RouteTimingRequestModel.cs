using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Route.RouteTiming
{
    public class RouteTimingRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int RouteTimingId { get; set; }
        public int RouteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime MorningTime { get; set; }
        public DateTime EveningTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? Is_Deleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
