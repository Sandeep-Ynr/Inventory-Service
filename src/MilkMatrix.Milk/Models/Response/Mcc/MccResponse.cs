using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Mcc
{
    public class MccResponse
    {
        public int? Id { get; set; }

        public string? PlantId { get; set; }
        public string? Name { get; set; }
        public string? MccCode { get; set; }
        public string? Capacity { get; set; }
        public string? FSSSINumber { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
