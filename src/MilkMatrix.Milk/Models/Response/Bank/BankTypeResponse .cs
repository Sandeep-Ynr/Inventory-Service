using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankTypeResponse : CommonLists
    {
        public int? BankTypeId { get; set; }
        public string? BankTypeName { get; set; }
        public string? BankTypeDescription { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifyOn { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}

