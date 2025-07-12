using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Api.Models.Request.Bank.BankType
{
    public class BankTypeUpdateRequestModel
    {
        public int BankTypeId { get; set; }
        public string BankTypeName { get; set; } = string.Empty;
        public string? BankTypeDescription { get; set; }
        public bool? IsStatus { get; set; }
    }
}
