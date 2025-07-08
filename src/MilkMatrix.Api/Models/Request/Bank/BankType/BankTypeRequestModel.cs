using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Api.Models.Request.Bank.BankType
{
    public class BankTypeRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? BankTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
