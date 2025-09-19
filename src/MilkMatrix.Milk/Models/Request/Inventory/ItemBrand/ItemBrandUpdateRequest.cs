using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Inventory.ItemBrand
{
    public class ItemBrandUpdateRequest
    {
        public int BrandId { get; set; }             // brand_id (PK)
        public string BusinessId { get; set; }       // NVARCHAR(50)
        public string Code { get; set; }             // VARCHAR(30)
        public string Name { get; set; }             // VARCHAR(100)
        public string Description { get; set; }      // VARCHAR(200)
        public bool IsActive { get; set; }           // BIT
        public int? ModifyBy { get; set; }           // INT NULL
        public DateTime? ModifyOn { get; set; }      // DATETIME NULL
    }
}
