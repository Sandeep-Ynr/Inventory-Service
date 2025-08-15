namespace MilkMatrix.Api.Models.Request.Route.RouteTiming
{
    public class RouteTimingInsertRequestModel
    {
        public int BusinessId { get; set; }
        public int RouteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime MorningTime { get; set; }
        public DateTime EveningTime { get; set; }
        public bool? IsActive { get; set; }
 
    }

}
