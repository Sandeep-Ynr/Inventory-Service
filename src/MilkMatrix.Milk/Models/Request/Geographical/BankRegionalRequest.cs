using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class BankRegionalRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int ? RegionalBankShortName { get; set;}
        public int BankRegionalId { get; set; }
        public int BankCode { get; set; }
        public string? RegionalBankName { get; set; }
        public int BankId { get; set; }
        public bool IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public bool? IsStatus { get; set; }
    }
}
