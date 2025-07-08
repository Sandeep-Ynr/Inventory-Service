using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Api.Models.Request.Bank.BankRegional;

public class BankRegionalModel
{
  
    public ReadActionType ActionType { get; set; }
    public bool IsActive { get; set; }
    public int RegionalID { get; set; }
  
}
