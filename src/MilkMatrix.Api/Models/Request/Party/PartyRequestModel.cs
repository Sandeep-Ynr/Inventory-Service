using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? PartyID { get; set; }
        public bool IsStatus { get; set; }
    }
}
