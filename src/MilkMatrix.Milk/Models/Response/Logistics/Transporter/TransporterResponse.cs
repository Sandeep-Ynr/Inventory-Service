using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Logistics.Transporter
{
    public class TransporterResponse : CommonLists
    {
        public string? TransporterName { get; set; }
        public string? LocalName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Pincode { get; set; }
        public string? RegistrationNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? LocalContactPerson { get; set; }
        public int BankID { get; set; }
        public string? BankName { get; set; }
        public int BranchID { get; set; }
        public string? BranchName { get; set; }
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
        public string? CompanyCode { get; set; }
        public int StateID { get; set; }
        public string? StateName { get; set; }
        public int DistrictID { get; set; }
        public string? DistrictName { get; set; }
        public int TehsilID { get; set; }
        public string? TehsilName { get; set; }
        public int VillageID { get; set; }
        public string? VillageName { get; set; }
        public int HamletID { get; set; }
        public int? VendorID { get; set; }
        public string? HamletName { get; set; }
        public decimal? SecurityAmount { get; set; }
        public bool? IsActive { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
