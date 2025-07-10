using System.ComponentModel.DataAnnotations;
using MilkMatrix.Core.Entities.Response;
namespace MilkMatrix.Milk.Models.Response.Bank
{
    public class BankTypeResponse : CommonLists
    {
        public string? BankTypeDescription { get; set; }
    }
}

