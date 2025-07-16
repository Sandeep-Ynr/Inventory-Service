using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.MPP
{
    public class MPPRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? MPPID { get; set; }
        public bool? IsActive { get; set; }
    }
}
