using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile
{
    public class MemberMilkProfileRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? MilkProfileID { get; set; }
        public long? MemberID { get; set; }
        public string? AnimalType { get; set; }
        public string? PreferredShift { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }
    }
}
