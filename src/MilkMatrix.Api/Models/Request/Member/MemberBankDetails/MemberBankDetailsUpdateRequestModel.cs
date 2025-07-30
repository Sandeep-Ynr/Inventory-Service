using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberBankDetails
{
    public class MemberBankDetailsUpdateRequestModel
    {
        public long BankDetailID { get; set; }
        public int BankID { get; set; }
        public int BranchID { get; set; }
        public string? AccountHolderName { get; set; }
        public string? AccountNumber { get; set; }
        public string? IFSCCode { get; set; }
        public bool IsJointAccount { get; set; }
        public string? PassbookFilePath { get; set; }
        public bool IsStatus { get; set; }
    }
}
