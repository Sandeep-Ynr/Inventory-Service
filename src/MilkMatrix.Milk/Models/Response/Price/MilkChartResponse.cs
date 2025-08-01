using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Price
{
    public class MilkChartResponse
    {
        public string? Results { get; set; }
        public List<MilkChartRow>? ChartData { get; set; }

        //public string? Fat { get; set; }
        //public Dictionary<string, object>? AdditionalData { get; set; }
    }

    public class MilkChartRow
    {
        public decimal Fat { get; set; }
        public Dictionary<decimal, decimal?> SNFValues { get; set; } = new();
    }
}
