using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberAddress
{
    public class MemberAddressUpdateRequestModel
    {
        public long AddressID { get; set; }
        public long MemberID { get; set; }
        public int StateID { get; set; }
        public int DistrictID { get; set; }
        public int TehsilID { get; set; }
        public int VillageID { get; set; }
        public int? HamletID { get; set; }
        public string FullAddress { get; set; }
        public string Pincode { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsStatus { get; set; }
    }
}
