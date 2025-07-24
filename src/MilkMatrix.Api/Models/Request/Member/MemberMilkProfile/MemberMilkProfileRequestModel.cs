using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Member.MemberMilkProfile
{
    public class MemberMilkProfileRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? MilkProfileID { get; set; }
        public long? MemberID { get; set; }
        public string? AnimalType { get; set; }
        public string? PreferredShift { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
