using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Hamlet
{
    public class HamletUpdateRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string HamletName { get; set; }
        public int? HamletId { get; set; }
        public int? VillageId { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
