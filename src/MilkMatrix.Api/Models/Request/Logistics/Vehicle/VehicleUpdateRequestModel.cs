namespace MilkMatrix.Api.Models.Request.Logistics.Vehicle
{
    public class VehicleUpdateRequestModel
    {
        public int VehicleID { get; set; }
        public string VehicleNo { get; set; } = string.Empty;
        public string? VehicleType { get; set; }
        public string? RCNo { get; set; }
        public string? EngineNo { get; set; }
        public string? ChassisNo { get; set; }
        public string? RegistrationDate { get; set; }
        public string? InsuranceNo { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? InsuranceExpiryDate { get; set; }
        public string? PermitNo { get; set; }
        public string? PermitValidTill { get; set; }
        public string? PollutionNo { get; set; }
        public string? PollutionExpiryDate { get; set; }
        public string? DriverName { get; set; }
        public string? DriverMobile { get; set; }
        public string? DriverLicenseNo { get; set; }
        public string? LicenseValidTill { get; set; }
        public int TransporterID { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public bool IsStatus { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
