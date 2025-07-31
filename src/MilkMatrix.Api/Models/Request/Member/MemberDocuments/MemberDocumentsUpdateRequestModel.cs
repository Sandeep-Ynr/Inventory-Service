using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsUpdateRequestModel
    {
        public int DocumentID { get; set; }
        public string? AadharCardBase64 { get; set; }
        public string? VoterIDBase64 { get; set; }
        public string? OtherDocumentBase64 { get; set; }
        public bool IsStatus { get; set; }
    }
}
