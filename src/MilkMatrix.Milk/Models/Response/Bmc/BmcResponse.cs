using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Bmc
{
    public class BmcResponse:CommonLists
    {
        //public int? Id { get; set; }
        //public string? Name { get; set; }
        public string? MccId { get; set; }
        public string? BmcCode { get; set; }
        public int? BusinessEntityId { get; set; }
        public string? BusinessEntityName { get; set; }
        public string? Capacity { get; set; }
        public string? FSSSINumber { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
    }
}
