namespace MilkMatrix.Api.Models.Request.Milk
{
    public class RateTypeUpdateRequestModel
    {
        public string RateTypeId { get; set; }
        public string? RateTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
