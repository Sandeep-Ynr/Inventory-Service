namespace MilkMatrix.Api.Models.Request.Admin.GlobleSetting.Sequance
{
    public class SequanceInsertRequestModel
    {
      
        public string HeadName { get; set; } = string.Empty;
        public string? Prefix { get; set; }
        public int? StartValue { get; set; }
        public int? StopValue { get; set; }
        public int? IncrementValue { get; set; }
      
    }
}
