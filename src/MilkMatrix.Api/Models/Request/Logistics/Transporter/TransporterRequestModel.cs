using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Logistics.Transporter
{
    public class TransporterRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string? TransporterID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
