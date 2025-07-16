using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.SahayakVSP
{
    public class SahayakVSPRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? SahayakID { get; set; }
        public bool? IsActive { get; set; }
    }
}
