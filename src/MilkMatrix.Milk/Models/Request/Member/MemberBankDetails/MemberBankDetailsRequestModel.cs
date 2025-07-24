using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberBankDetails
{
    public class MemberBankDetailsRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? BankDetailID { get; set; }
        public long? MemberID { get; set; }
        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }
    }
}
