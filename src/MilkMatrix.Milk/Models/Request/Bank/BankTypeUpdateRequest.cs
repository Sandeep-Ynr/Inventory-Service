using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Milk.Models.Request.Bank
{
    public class BankTypeUpdateRequest
    {
        public int BankTypeId { get; set; }
        public string BankTypeName { get; set; } = string.Empty;
        public string? BankTypeDescription { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
