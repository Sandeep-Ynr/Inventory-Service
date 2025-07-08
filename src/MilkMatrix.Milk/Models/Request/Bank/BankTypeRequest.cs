using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Bank
{
    public class BankTypeRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public int? BankTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
