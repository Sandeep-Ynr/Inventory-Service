using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Member
{
    public class MemberRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string? MemberID { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public long? SocietyID { get; set; }
        public string? MobileNo { get; set; }
        public string? AadharNo { get; set; }
    }
}
