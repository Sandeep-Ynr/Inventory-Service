using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BranchResponse : CommonLists
    {
        public int BankID { get; set; }
        public string? BankName { get; set; }

        public int StateID { get; set; }
        public string? StateName { get; set; }


        

            
        public int BranchID { get; set; }
    }
}
