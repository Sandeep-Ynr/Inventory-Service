using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberBankDetails
{
    public class MemberBankDetailsInsertRequestModel
    {
        public long MemberID { get; set; }
        public int BankID { get; set; }
        public int BranchID { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public bool IsJointAccount { get; set; } = false;
        public string? PassbookFilePath { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
