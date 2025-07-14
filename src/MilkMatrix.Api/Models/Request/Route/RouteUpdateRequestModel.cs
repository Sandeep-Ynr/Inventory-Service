namespace MilkMatrix.Api.Models.Request.Route
{
    public class RouteUpdateRequestModel
    {
        public int RouteID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RouteCode { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string? RegionalName { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public int VehicleCapacity { get; set; }
        public DateTime MorningStartTime { get; set; }
        public DateTime MorningEndTime { get; set; }
        public DateTime EveningStartTime { get; set; }
        public DateTime EveningEndTime { get; set; }
        public decimal TotalKm { get; set; }
        public long? ModifyBy { get; set; }
        public bool IsStatus { get; set; }
    }
}
