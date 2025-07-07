using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class HamletResponse : CommonLists
    {
        public int Serial { get; set; } 
        public int HamletId { get; set; } 
        public string HamletName { get; set; } = string.Empty;
        public int VillageId { get; set; } 
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; } 
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; } 
        public DateTime? ModifyOn { get; set; }
    }
}
