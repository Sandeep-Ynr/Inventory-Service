using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankRegResponse : CommonLists
    {
        public int BankID { get; set; }
        public string? BankName { get; set; }
        public string RegionalCode { get; set; } = string.Empty;
        public string RegionalBankShortName { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
        public bool? IsStatus { get; set; }
    }
}
