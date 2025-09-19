using MilkMatrix.Core.Entities.Enums;
namespace MilkMatrix.Api.Models.Request.Inventory.ItemBrand
{
    public class ItemBrandUpdateRequestModel
    {
        public int BrandId { get; set; }             // brand_id (PK)
        public string BusinessId { get; set; }       // NVARCHAR(50)
        public string Name { get; set; }             // VARCHAR(100)
        public string Description { get; set; }      // VARCHAR(200)
        public bool IsActive { get; set; }           // BIT
        //public bool IsDeleted { get; set; }          // BIT
        //public int? CreatedBy { get; set; }          // INT NULL
        //public DateTime? CreatedOn { get; set; }     // DATETIME NULL
        //public int? ModifyBy { get; set; }           // INT NULL
        //public DateTime? ModifyOn { get; set; }      // DATETIME NULL
    }
}
