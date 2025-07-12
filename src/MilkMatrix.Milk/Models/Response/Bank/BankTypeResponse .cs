using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankTypeResponse : CommonLists
    {
        public string? BankTypeDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public long? ModifyBy { get; set; }
        public bool is_status { get; set; }
    }
}

