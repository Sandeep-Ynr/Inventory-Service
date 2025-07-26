namespace MilkMatrix.Milk.Models.Request.Member.MemberDocuments
{
    public class MemberDocumentsInsertRequest
    {
        public long MemberID { get; set; }
        public string? AadharFile { get; set; }
        public string? VoterOrRationCard { get; set; }
        public string? OtherDocument { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
