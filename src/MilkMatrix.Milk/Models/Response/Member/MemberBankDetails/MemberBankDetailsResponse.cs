using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member.MemberBankDetails
{
    public class MemberBankDetailsResponse : CommonLists
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
        public bool is_status { get; set; }
        public DateTime created_on { get; set; }
        public long created_by { get; set; }
        public DateTime? modify_on { get; set; }
        public long? modify_by { get; set; }
        public bool is_deleted { get; set; }
    }
}
