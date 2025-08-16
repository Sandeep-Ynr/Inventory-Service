using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Plant
{
    public class PlantResponse
    {
        public int? Id { get; set; }
        public string? PlantCode { get; set; }
        public string? Name { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Capacity { get; set; }
        public string? FSSSINumber { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
