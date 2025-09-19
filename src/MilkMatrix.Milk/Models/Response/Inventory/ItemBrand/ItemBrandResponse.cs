using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Response.Inventory.ItemBrand
{
   public class ItemBrandResponse
    {
        
            public int BrandId { get; set; }
            public string BusinessId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? ModifyBy { get; set; }
            public DateTime? ModifyOn { get; set; }
        

    }
}
