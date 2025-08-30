using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.PriceApplicability
{
    public class  PriceActualRateResponse
    {
        public decimal ActualRate { get; set; }
        public decimal Fat { get; set; }
        public decimal Snf { get; set; }
        public DateTime CalculatedOn { get; set; }
        public int RateCode { get; set; }
        public string? RateSource { get; set; }
    }
}
