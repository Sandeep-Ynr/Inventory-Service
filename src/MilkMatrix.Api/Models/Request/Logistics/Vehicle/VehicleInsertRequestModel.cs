namespace MilkMatrix.Api.Models.Request.Logistics.Vehicle
{
    public class VehicleInsertRequestModel
    {
        public int VehicleTypeId { get; set; }
        public string? CapacityCode { get; set; }
        public string? RegistrationNo { get; set; }
        public string? ApplicableRTO { get; set; }
        public string? DriverName { get; set; }
        public string? DriverContactNo { get; set; }
        public DateTime WEFDate { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? LicenceExpiryDate { get; set; }
        public string ?TransporterCode { get; set; }
        public string ?MappedRoute { get; set; }
        public string ?PollutionCertificate { get; set; }
        public string ?Insurance { get; set; }
        public string ?RCBookNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? Rent { get; set; }
        public decimal? Average { get; set; }
        public string ?CompanyCode { get; set; }
        public string ?FuelTypeCode { get; set; }
        public string ?PassingNo { get; set; }
        public string ?BMCCode { get; set; }
        public bool IsStatus { get; set; }
    }
}
