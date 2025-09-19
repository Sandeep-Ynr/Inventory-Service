using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Inventory.ItemBrand
{
   public class ItemBrandInsertRequest
    {
        public string BusinessId { get; set; }       // NVARCHAR(50)
        public string Code { get; set; }             // VARCHAR(30)
        public string Name { get; set; }             // VARCHAR(100)
        public string Description { get; set; }      // VARCHAR(200)
        public bool IsActive { get; set; }           // BIT
        public bool IsDeleted { get; set; }          // BIT
        public int? CreatedBy { get; set; }          // INT NULL
        public DateTime? CreatedOn { get; set; }     // DATETIME NULL
    }
}
