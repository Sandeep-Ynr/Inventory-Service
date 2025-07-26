namespace MilkMatrix.Milk.Models.Response.Member.MemberDocuments
{
    public class MemberDocumentsResponse
    {
        public long DocumentID { get; set; }
        public long MemberID { get; set; }
        public string? AadharFile { get; set; }
        public string? VoterOrRationCard { get; set; }
        public string? OtherDocument { get; set; }
        public bool is_status { get; set; }
        public DateTime created_on { get; set; }
        public long created_by { get; set; }
        public DateTime? modify_on { get; set; }
        public long? modify_by { get; set; }
        public bool is_deleted { get; set; }
    }
}