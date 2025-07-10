using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankRegResponse : CommonLists
    {
        public int BankID { get; set; }
        public string? BankName { get; set; }
    }
}
