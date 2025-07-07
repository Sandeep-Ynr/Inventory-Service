using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class BankRegUpdateRequest
    {
        public int BankCode { get; set; }
        public int RegionalID { get; set; }
        public int RegionalCode { get; set; }
        public string? RegionalBankName { get; set; }
        public string? RegionalBankShortName { get; set;}
        public int? ModifyBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
