namespace MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance
{
    public class SequanceInsertRequestModel
    {
      
        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? CreatedBy { get; set; }

    }

    public class SequanceTransInsertRequestModel
    {

        public string HeadName { get; set; } = string.Empty;
        public string? fy_year { get; set; }
        public string? delimiter { get; set; }
        public string? Prefix { get; set; }
        public string? suffix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
        public int? CreatedBy { get; set; }

    }
}
