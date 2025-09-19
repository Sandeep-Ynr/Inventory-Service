using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Queries
{
    public class ItemCatgQueries
    {
            public const string AddItemCatg = "usp_item_category_insupd";
            public const string GetCatgList = "usp_item_category_list";

    }

    public class ItemQueries
    {
        public const string AddItem = "usp_item_insupd";
        public const string GetItemList = "usp_item_list";

    }

    public class ItemBrand
    {
        public const string InsUpdItem = "usp_brand_master_insupd";
        public const string GetItemList = "usp_brand_list";

    }

    //usp_brand_master_insupd
}


