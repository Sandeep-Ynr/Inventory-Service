using System;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Logistics.Vendor
{
    public class VendorResponse : CommonLists
    {
        public string? VendorCode { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? GSTIN { get; set; }
        public string? PanNo { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
    }
}
