using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Inventory.Item
{
    public class ItemResponse
    {
        public long ItemId { get; set; }
        public int BusinessId { get; set; }
        public int ItemTypeId { get; set; }
        public int LifecycleStatusId { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int BaseUomId { get; set; }
        public bool Is_Perishable { get; set; }
        public bool Is_Batch_Tracked { get; set; }
        public bool Is_Serial_Tracked { get; set; }
        public string Hsn_Sac { get; set; }
        public decimal? Mrp { get; set; }
        public decimal? Purchase_Rate { get; set; }
        public decimal? Sale_Rate { get; set; }
        public decimal? Avg_Rate { get; set; }
        public string Barcode { get; set; }
        public string Brand { get; set; }
        public string Notes { get; set; }
        public bool Is_Active { get; set; }
        public bool? is_deleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }
}
