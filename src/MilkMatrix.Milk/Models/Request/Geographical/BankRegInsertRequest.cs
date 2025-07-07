using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class BankRegInsertRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public string? RegionalCode { get; set; }
        public string? BankID { get; set; }
        public string? RegionalBankName { get; set; }
        public string? RegionalBankShortName { get; set;}
        public int? CreatedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
