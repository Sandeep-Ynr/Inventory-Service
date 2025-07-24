using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberBankDetails
{
    public class MemberBankDetailsUpdateRequest
    {
        public long BankDetailID { get; set; }
        public long MemberID { get; set; }
        public int BankID { get; set; }
        public int BranchID { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public bool IsJointAccount { get; set; }
        public string? PassbookFilePath { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
