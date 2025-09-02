namespace MilkMatrix.Api.Models.Request.Logistics.Route
{
    public class RouteUpdateRequestModel
    {
        public int RouteID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RouteCode { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string? RegionalName { get; set; }
        public int? BmcId { get; set; }
        public int? RouteContractorId { get; set; }
        public int VehicleID { get; set; }
        public int VehicleCapacity { get; set; }
        public TimeSpan? MorningStartTime { get; set; }
        public TimeSpan? MorningEndTime { get; set; }
        public TimeSpan? EveningStartTime { get; set; }
        public TimeSpan? EveningEndTime { get; set; }
        public decimal TotalKm { get; set; }
        public long? ModifyBy { get; set; }
        public bool IsStatus { get; set; }
    }
}
