
namespace MilkMatrix.Api.Models.Request.Route.RouteTiming
{
    public class RouteTimingUpdateRequestModel
    {
        public int RouteTimingId { get; set; }
        public int RouteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime MorningTime { get; set; }
        public DateTime EveningTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
