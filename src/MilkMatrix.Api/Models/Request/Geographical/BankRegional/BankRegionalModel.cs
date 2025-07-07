using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Enums;
//using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Geographical.BankRegional;

public class BankRegionalModel
{
  
    public ReadActionType ActionType { get; set; }
    public bool IsActive { get; set; }
    public int RegionalID { get; set; }
  
}
