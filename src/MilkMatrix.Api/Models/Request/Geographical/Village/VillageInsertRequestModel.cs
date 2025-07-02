using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageInsertRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;

        public int? VillageId { get; set; }

        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
    
        public int? CreatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
