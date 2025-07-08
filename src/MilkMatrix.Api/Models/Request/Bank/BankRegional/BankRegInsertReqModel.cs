using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Bank.BankRegional;

public class BankRegInsertReqModel
{
    public int RegionalCode { get; set; }
    public int BankID { get; set; }
    public string RegionalBankName { get; set; } = string.Empty;
    public string RegionalBankShortName { get; set; } = string.Empty;
    public int? CreatedBy { get; set; }
    public bool? IsStatus { get; set; }
}
