
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Route.RouteTiming
{
    public class RouteTimingResponse:CommonLists
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int RouteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public TimeSpan MorningTime { get; set; }
        public TimeSpan EveningTime { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
