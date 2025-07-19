namespace MilkMatrix.Api.Models.Request.Logistics.Vehicle
{
    public class VehicleInsertRequestModel
    {
        public string VehicleNo { get; set; } = string.Empty;
        public string? LocalVehicleName { get; set; }
        public string? VehicleType { get; set; }
        public string? ChassisNo { get; set; }
        public string? EngineNo { get; set; }
        public string? ModelNo { get; set; }
        public string? Manufacturer { get; set; }
        public string? FuelType { get; set; }
        public string? RCBookNo { get; set; }
        public string? PermitNo { get; set; }
        public DateTime? PermitValidTill { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int TransporterID { get; set; }
        public string? InsurancePolicyNo { get; set; }
        public DateTime? InsuranceValidTill { get; set; }
        public string? PollutionCertNo { get; set; }
        public DateTime? PollutionValidTill { get; set; }
        public string? GPSDeviceNo { get; set; }
        public string? DriverName { get; set; }
        public string? DriverMobileNo { get; set; }
        public string? Remarks { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public bool? IsStatus { get; set; }
    }
}
