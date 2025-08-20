namespace MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance
{
    public class SequenceInsertRequest
    {

        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? CreatedBy { get; set; }

    }

    public class SequenceTransInsertRequest
    {
        public string HeadName { get; set; } = string.Empty;
        public string fy_year { get; set; }
        public string? Prefix { get; set; }
        public string? suffix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public string? delimiter { get; set; }
        public int? CreatedBy { get; set; }
        
    }
}
