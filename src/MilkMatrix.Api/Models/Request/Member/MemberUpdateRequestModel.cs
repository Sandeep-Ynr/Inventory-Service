namespace MilkMatrix.Api.Models.Request.Member
{
    public class MemberUpdateRequestModel
    {
        public int? MemberID { get; set; }
        public string? MemberCode { get; set; }
        public string? FarmerName { get; set; }
        public string? LocalName { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MobileNo { get; set; }
        public string? AlternateNo { get; set; }
        public string? EmailID { get; set; }
        public string? AadharNo { get; set; }
        public long SocietyID { get; set; }
        public bool? IsStatus { get; set; }
    }
}
