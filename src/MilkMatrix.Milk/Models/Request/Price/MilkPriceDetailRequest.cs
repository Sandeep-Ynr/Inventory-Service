using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Price
{
    public class MilkPriceDetailRequest
    {
        public string? fat { get; set; }
        public double? rate { get; set; }
        public string? snf { get; set; }
    }
}
