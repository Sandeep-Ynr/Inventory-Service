namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class RateForUpdateRequestModel
    {
        public string RateForId { get; set; }
        public string? RateForCode { get; set; }
        public string? RateForName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
