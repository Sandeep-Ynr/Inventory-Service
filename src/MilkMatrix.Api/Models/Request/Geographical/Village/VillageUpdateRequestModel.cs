using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageUpdateRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? VillageId { get; set; }
        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsStatus { get; set; }
        public int? ModifyBy { get; set; }
    }
}
