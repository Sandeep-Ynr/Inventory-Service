using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyGroupRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? GroupId { get; set; }
        public bool? IsStatus { get; set; }
    }
}
