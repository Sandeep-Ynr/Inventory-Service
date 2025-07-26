using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsUpdateRequestModel
    {
        public long DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharFile { get; set; }
        public string? VoterOrRationCard { get; set; }
        public string? OtherDocument { get; set; }
        public bool IsStatus { get; set; }
    }
}
