using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberAddress
{
    public class MemberAddressRequestModel
    {
        public ReadActionType? ActionType { get; set; } = ReadActionType.All;
        public long? AddressID { get; set; }
        public long? MemberID { get; set; }
        public int? StateID { get; set; }
        public int? DistrictID { get; set; }
        public int? TehsilID { get; set; }
        public int? VillageID { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }
    }
}
