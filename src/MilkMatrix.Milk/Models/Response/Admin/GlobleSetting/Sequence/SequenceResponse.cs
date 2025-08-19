using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Admin.GlobleSetting.Sequance
{
    public class SequenceResponse : CommonLists
    {
        public int Serial { get; set; }
        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? LastValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }

    public class NextNumberResponse 
    {

        public string HeadName { get; set; } = string.Empty;
        public int UpdatedValue { get; set; }
        public string ?Message { get; set; }
    }

    public class SequenceTransResponse : CommonLists
    {
        public int Serial { get; set; }
        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? LastValue { get; set; }
        public string? FinancialYear { get; set; }
        public string? Suffix { get; set; }

        public string? delimiter { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }

    public class SeqTransNextNumberResponse
    {

        public string HeadName { get; set; } = string.Empty;
        public int UpdatedValue { get; set; }
        public string ? UpdatedCode { get; set; }
        public string? Message { get; set; }
    }
}
