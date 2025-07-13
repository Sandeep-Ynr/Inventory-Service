namespace MilkMatrix.Api.Models.Request.Bank.Branch
{
    public class BranchRequestModel
    {
        public string BranchCode { get; set; } = string.Empty;
        public int BankID { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string? LocalBranchName { get; set; }
        public string IFSC { get; set; } = string.Empty;
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int VillageID { get; set; }
        public int HamletID { get; set; }
        public string? Address { get; set; }
        public string? AddressHindi { get; set; }
        public string? Pincode { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNo { get; set; }
        public bool? IsStatus { get; set; }
        public int CreatedBy { get; set; }
    }
}
