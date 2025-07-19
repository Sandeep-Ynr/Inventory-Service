using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Logistics.Transporter
{
    public class TransporterUpdateRequest
    {
        public string TransporterID { get; set; } = string.Empty;
        public string TransporterName { get; set; } = string.Empty;
        public string? LocalName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Pincode { get; set; }
        public string? RegistrationNo { get; set; }
        public string ContactPerson { get; set; } = string.Empty;
        public string? LocalContactPerson { get; set; }
        public int BankID { get; set; }
        public int BranchID { get; set; }
        public string? BranchCode { get; set; }
        public string? BankAccountNo { get; set; }
        public string? IFSC { get; set; }
        public string? GSTIN { get; set; }
        public decimal? TdsPer { get; set; }
        public string? PanNo { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? AgreementNo { get; set; }
        public string? Declaration { get; set; }
        public string? SecurityChequeNo { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int VillageID { get; set; }
        public int HamletID { get; set; }
        public decimal? SecurityAmount { get; set; }

        public int VendorID { get; set; }
        
        public string? VendorName { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsStatus { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
