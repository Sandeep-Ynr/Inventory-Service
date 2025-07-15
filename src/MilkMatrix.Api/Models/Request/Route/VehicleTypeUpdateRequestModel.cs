namespace MilkMatrix.Api.Models.Request.Route
{
    public class VehicleTypeUpdateRequestModel
    {
        public int VehicleID { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
