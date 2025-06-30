using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;

        public int? VillageId { get; set; }

        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        //public int? DistrictId { get; set; }

        //public int? StateId { get; set; }

        public bool? IsActive { get; set; }
    }
}
