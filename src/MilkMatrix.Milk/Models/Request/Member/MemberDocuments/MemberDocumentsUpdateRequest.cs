namespace MilkMatrix.Milk.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsUpdateRequest
    {
        public int DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharCardBase64 { get; set; }
        public string? VoterIDBase64 { get; set; }
        public string? OtherDocumentBase64 { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
