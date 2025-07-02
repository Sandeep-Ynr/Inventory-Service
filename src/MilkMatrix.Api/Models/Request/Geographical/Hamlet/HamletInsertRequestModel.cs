using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Hamlet
{
    public class HamletInsertRequestModel
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;

        public int? HamletId { get; set; }
        public string? HamletName { get; set; }
        public int? VillageId { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
