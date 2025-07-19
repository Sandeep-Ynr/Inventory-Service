using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Logistics.Vehicle
{
    public class  VehicleUpdateRequest
    {
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public string CapacityCode { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string? ApplicableRTO { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string? DriverContactNo { get; set; }
        public DateTime WEFDate { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? LicenceExpiryDate { get; set; }
        public string TransporterCode { get; set; } = string.Empty;
        public string? MappedRoute { get; set; }
        public string? PollutionCertificate { get; set; }
        public string? Insurance { get; set; }
        public string? RCBookNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? Rent { get; set; }
        public decimal? Average { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string FuelTypeCode { get; set; } = string.Empty;
        public string PassingNo { get; set; } = string.Empty;
        public string? BMCCode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? ModifiedOn { get; set; } = DateTime.UtcNow;
        public int? ModifiedBy { get; set; }
    }
}
