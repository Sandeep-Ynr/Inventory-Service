using MilkMatrix.Core.Entities.Enums; // Assuming ReadActionType is here
using System.ComponentModel.DataAnnotations; // For data annotations

namespace MilkMatrix.Api.Models.Request.Logistics.VehicleBillingType
{
    public class VehicleBillingTypeRequestModel
    {
        public long TypeId { get; set; }
        public long VehicleId { get; set; }
        public long BillingTypeId { get; set; }
        public DateTime WefDate { get; set; }
        public string? Remarks { get; set; }
        public long BusinessId { get; set; }
        public long TransporterId { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }
}
