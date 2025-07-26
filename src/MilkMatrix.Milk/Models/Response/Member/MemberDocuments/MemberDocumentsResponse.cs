namespace MilkMatrix.Milk.Models.Response.Member.MemberDocuments
{
    public class MemberDocumentsResponse
    {
        public int DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharCardBase64 { get; set; }
        public string? VoterIDBase64 { get; set; }
        public string? OtherDocumentBase64 { get; set; }
        public bool is_status { get; set; }
        public DateTime created_on { get; set; }
        public long created_by { get; set; }
        public DateTime? modify_on { get; set; }
        public long? modify_by { get; set; }
        public bool is_deleted { get; set; }
    }
}
