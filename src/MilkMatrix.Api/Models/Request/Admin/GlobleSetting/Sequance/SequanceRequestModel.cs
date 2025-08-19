namespace MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance
{
    public class SequanceRequestModel
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

    public class SequanceTransRequestModel
    {
        public int Serial { get; set; }
        public string HeadName { get; set; } = string.Empty;
        public string fy_year { get; set; }
        public string? Prefix { get; set; }
        public string? suffix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? LastValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }

    }
}
