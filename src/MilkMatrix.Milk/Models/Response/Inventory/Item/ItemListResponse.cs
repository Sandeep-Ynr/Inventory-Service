using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Inventory.Item
{
    public class ItemListResponse
    {
        public int BusinessId { get; set; }
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }   // main category
        public int? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } // sub category
        public string BaseUOMId { get; set; }       // because measurement_unit_id is nvarchar(10)
        public string BaseUOMCode { get; set; }     // unit code
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int? BrandId { get; set; }
        public decimal Mrp { get; set; }
        public decimal SaleRate { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
    }
}
