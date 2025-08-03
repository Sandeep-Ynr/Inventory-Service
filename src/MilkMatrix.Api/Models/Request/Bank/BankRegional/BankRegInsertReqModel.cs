using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Bank.BankRegional;

public class BankRegInsertReqModel
{
    public string? BusinessId { get; set; }
    public int BankID { get; set; }
    public int RegionalCode { get; set; }
    public string RegionalBankName { get; set; } = string.Empty;
    public string RegionalBankShortName { get; set; } = string.Empty;
    public bool? IsStatus { get; set; }
}
