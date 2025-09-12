using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Inventory.ItemCategory
{
   public class ItemCatgInsertRequest
    {
        public int Id { get; set; }
        public int? BusinessId { get; set; }
        public int? ParentId { get; set; }
        public string? Code { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }
}
