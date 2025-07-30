using MilkMatrix.Api.Models.Request.Member.MemberAddress;
using MilkMatrix.Api.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Api.Models.Request.Member.MemberDocuments;
using MilkMatrix.Api.Models.Request.Member.MemberMilkProfile;

namespace MilkMatrix.Api.Models.Request.Member
{
    public class MemberInsertRequestModel
    {
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

        public List<MemberAddressInsertRequestModel>? addressList { get; set; }
        public List<MemberBankDetailsInsertRequestModel>? bankList { get; set; }
        public List<MemberMilkProfileInsertRequestModel>? MilkProfileList { get; set; }
        public List<MemberDocumentsInsertRequestModel>? MilkDocumentList { get; set; }


    }
}
