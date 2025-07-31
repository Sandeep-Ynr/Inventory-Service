using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Logistics.VehicleBillingType
{
    public class VehicleBillingTypeRequest
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
