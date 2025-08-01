using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Api.Models.Request.Bank.Bank
{
    public class BankInsertRequestModel
    {
        public string? BusinessId { get; set; }
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
        public bool? IsStatus { get; set; }
    }
}
