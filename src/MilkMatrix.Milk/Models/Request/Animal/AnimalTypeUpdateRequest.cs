using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Animal
{
    public class AnimalTypeUpdateRequest
    {
        public string? AnimalTypeId { get; set; }
        public string? AnimalTypeCode { get; set; }
        public string? AnimalTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifyBy { get; set; }
    }
}
