using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Price
{
    public class MilkPriceDetailRequest
    {
        public string? Fat { get; set; }
        public double? Price { get; set; }
        public string? SNF { get; set; }
    }
}
