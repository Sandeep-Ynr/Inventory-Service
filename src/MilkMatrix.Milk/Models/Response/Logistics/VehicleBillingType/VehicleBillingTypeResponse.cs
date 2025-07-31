using System;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Logistics.VehicleBillingType
{
    public class VehicleBillingTypeResponse : CommonLists
    {
        public long VehicleBillingCode { get; set; }
        public string? VehicleCode { get; set; }
        public long BillingTypeCode { get; set; }
        public DateTime WefDate { get; set; }
        public string? Remarks { get; set; }
        public long TransporterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeleteBy { get; set; }
    }
}
