namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class RateForInsertRequestModel
    {
        public string? RateForCode { get; set; }
        public string? RateForName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
