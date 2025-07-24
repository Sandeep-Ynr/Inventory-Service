using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member.MemberAddress
{
    public class MemberAddressResponse : CommonLists
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
        public bool is_status { get; set; }
        public DateTime created_on { get; set; }
        public long created_by { get; set; }
        public DateTime? modify_on { get; set; }
        public long? modify_by { get; set; }
        public bool is_deleted { get; set; }
    }
}
