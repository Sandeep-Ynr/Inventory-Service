using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public class PriceApplicabilityQueries
    {
        public static class PriceApplicabilityQuery
        {
            public const string GetPriceAppList = "usp_price_applicability_list";
            public const string AddPriceApp = "usp_price_applicability_insupd";
        }
    }
}
