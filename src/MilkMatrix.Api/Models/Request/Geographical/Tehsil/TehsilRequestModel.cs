using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Tehsil
{
    public class TehsilRequestModel
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }
        public int DistrictId { get; set; }
        public bool? IsActive { get; set; }
        
    }
}
