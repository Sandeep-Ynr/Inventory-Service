using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankResponse : CommonLists
    {
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string? BankShortName { get; set; }
        public int? BankTypeID { get; set; }
        public string? BankTypeName { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? AccountNoLength { get; set; }
        public bool? IsAccountValidationEnabled { get; set; }
        public bool? IsNationalized { get; set; }
    }
}

