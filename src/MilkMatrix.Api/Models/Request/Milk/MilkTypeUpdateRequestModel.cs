namespace MilkMatrix.Api.Models.Request.Milk
{
    public class MilkTypeUpdateRequestModel
    {
        public string MilkTypeId { get; set; }
        public string? MilkTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
