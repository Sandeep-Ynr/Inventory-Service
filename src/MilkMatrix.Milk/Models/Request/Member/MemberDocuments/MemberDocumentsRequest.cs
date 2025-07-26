namespace MilkMatrix.Milk.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsRequest
    {
        public int DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharCardBase64 { get; set; }
        public string? VoterIDBase64 { get; set; }
        public string? OtherDocumentBase64 { get; set; }
    }
}
