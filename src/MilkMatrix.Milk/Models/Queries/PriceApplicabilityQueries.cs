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
            //public const string GetPriceAppList = "usp_price_applicability_list";
            public const string GetPriceAppList = "usp_rate_mapping_list";
            
            public const string AddPriceApp = "usp_price_applicability_insupd";
            public const string InsupRateMapping = "usp_rate_mapping_insupd_v1";
        }

        public static class RateForQuery
        {
            public const string GetRateForList = "usp_get_actual_rate";
            public const string AddRateFor = "usp_rate_For_insupd";
        }
    }
}
