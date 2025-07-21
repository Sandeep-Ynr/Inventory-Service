using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Logistics.Vendor
{
    public class VendorRequest
    {
        public string? VendorCode { get; set; }
        public string? VendorName { get; set; }
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? GSTIN { get; set; }
        public string? PanNo { get; set; }
        public string? Address { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
    }
}
