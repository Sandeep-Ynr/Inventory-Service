using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Milk.Models.Queries
{
    public class BmcQueries
    {
        public static class BmcQuery
        {
            public const string GetBmcList = "usp_bmc_list";
            public const string AddBmc = "usp_bmc_insupd";
        }
    }
}
