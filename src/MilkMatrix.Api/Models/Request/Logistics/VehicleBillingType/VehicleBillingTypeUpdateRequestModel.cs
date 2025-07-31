using System;
using System.ComponentModel.DataAnnotations; // For data annotations

namespace MilkMatrix.Api.Models.Request.Logistics.VehicleBillingType
{
    public class VehicleBillingTypeUpdateRequestModel
    {
        public long TypeId { get; set; }
        public long VehicleId { get; set; }
        public long BillingTypeId { get; set; }
        public DateTime WefDate { get; set; }
        public string? Remarks { get; set; }
        public long BusinessId { get; set; }
        public long TransporterId { get; set; }
        public bool IsStatus { get; set; }
    }
}
