using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public static partial class PriceQueries
    {
        public static class PriceQuery       {
            public const string MilkPriceList = "usp_milk_price_list"; //usp_price_list
            public const string InsupdMilkPrice = "usp_milk_price_insupd"; //usp_price_insupd
            public const string InsupdMilkPriceDetail = "usp_milk_price_detail_insupd";
            public const string MilkRateChart = "usp_milk_price_chart";

            public const string UserStagingTable = "tbl_staging_bulk_users"; //for bulk user staging table
            public const string ProcessStagedUsers = "usp_process_bulk_users"; //For processing staged users
            public const string GetFailedBulkProcessingUsers = "SELECT * FROM tbl_staging_bulk_users WHERE ProcessStatus = 'Failed'"; //For getting failed processing users
        }
    }
}
