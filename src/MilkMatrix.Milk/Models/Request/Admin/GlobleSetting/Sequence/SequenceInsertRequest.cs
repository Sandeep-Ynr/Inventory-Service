namespace MilkMatrix.Milk.Models.Request.Admin.GlobleSetting.Sequance
{
    public class SequenceInsertRequest
    {
    
        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? LastValue { get; set; }
        public int? CreatedBy { get; set; }

    }
}
