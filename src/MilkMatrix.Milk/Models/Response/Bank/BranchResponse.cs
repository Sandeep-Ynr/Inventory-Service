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
        public int DistrictID { get; set; }
        public string? DistrictName { get; set; }
        public int TehsilID { get; set; }
        public string? TehsilName { get; set; }
        public int VillageID { get; set; }
        public string? VillageName { get; set; }
        public int HanletID { get; set; }
        public string? HamletName { get; set; }
    }
}
