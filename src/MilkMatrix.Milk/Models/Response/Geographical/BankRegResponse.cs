using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class BankRegResponse : CommonLists
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int RegionalID { get; set; }
        public string? RegionalCode { get; set; }
        public int BankID { get; set; }
        public string? RegionalBankName { get; set; }
        public string? RegionalBankShortName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
