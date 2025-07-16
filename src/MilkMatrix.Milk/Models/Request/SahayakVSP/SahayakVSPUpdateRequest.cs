using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.SahayakVSP
{
    public class SahayakVSPUpdateRequest
    {
        public int SahayakID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public string MPPCode { get; set; } = string.Empty;
        public string SahayakName { get; set; } = string.Empty;
        public string? ShortName { get; set; }
        public string? RegionalName { get; set; }
        public string ExSahayakCode { get; set; } = string.Empty;
        public string? RegistrationNo { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? Logo { get; set; }
        public string? PunchLine { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int VillageID { get; set; }
        public int HamletID { get; set; }
        public string? Address { get; set; }
        public string? RegionalAddress { get; set; }
        public string? Pincode { get; set; }
        public string MobileNo { get; set; } = string.Empty;
        public string? PhoneNo { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactRegionalName { get; set; }
        public string? Pancard { get; set; }
        public int BankID { get; set; }
        public int BranchID { get; set; }
        public string? AccNo { get; set; }
        public string? IFSC { get; set; }
        public bool IsStatus { get; set; }
        public long? ModifiedBy { get; set; }
    }
}

