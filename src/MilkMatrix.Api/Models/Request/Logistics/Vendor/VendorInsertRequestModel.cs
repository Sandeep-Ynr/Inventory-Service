namespace MilkMatrix.Api.Models.Request.Logistics.Vendor
{
    public class VendorInsertRequestModel
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

    }
}
