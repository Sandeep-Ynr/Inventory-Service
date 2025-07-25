using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberAddress
{
    public class MemberAddressUpdateRequest
    {
        public long AddressID { get; set; }
        public long MemberID { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int VillageID { get; set; }
        public int? HamletID { get; set; }
        public string? FullAddress { get; set; }
        public string? Pincode { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
