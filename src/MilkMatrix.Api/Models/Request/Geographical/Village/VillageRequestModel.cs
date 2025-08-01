using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? BusinessId { get; set; }
        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
    }
}
