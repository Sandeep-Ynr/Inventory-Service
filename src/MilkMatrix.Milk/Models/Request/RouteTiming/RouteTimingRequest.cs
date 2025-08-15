
using System;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Route.RouteTiming
{
    public class RouteTimingRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int BusinessId { get; set; }
        public int RouteTimingId { get; set; }
        public int RouteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime MorningTime { get; set; }
        public DateTime EveningTime { get; set; }
        public bool? IsStatus { get; set; }
        public bool? Is_Deleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
