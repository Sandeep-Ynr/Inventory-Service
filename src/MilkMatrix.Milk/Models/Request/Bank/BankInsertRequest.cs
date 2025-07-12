using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Milk.Models.Request.Bank
{
    public class BankInsertRequest
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string? BankShortName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? BankTypeID { get; set; }
        public int? AccountNoLength { get; set; }
        public bool? IsAccountValidationEnabled { get; set; }
        public bool? IsNationalized { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
    }
}
