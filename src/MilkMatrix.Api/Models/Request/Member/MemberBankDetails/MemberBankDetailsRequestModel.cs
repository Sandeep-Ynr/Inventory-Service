using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Member.MemberBankDetails
{
    public class MemberBankDetailsRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? BankDetailID { get; set; }
        public long? MemberID { get; set; }
        public int? BankID { get; set; }
        public int? BranchID { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
