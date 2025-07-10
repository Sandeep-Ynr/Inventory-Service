using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankResponse : CommonLists
    {
        public int? BankTypeID { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string? BankShortName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? AccountNoLength { get; set; }
        public bool? IsAccountValidationEnabled { get; set; }
        public bool? IsNationalized { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public bool? IsStatus { get; set; }
    }
}

