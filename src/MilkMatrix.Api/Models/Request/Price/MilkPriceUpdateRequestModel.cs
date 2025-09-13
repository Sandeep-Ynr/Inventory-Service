namespace MilkMatrix.Api.Models.Request.Price
{
    public class MilkPriceUpdateRequestModel
    {
        public string? RateCode { get; set; }
        public int? BusinessEntityId { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public string? ShiftId { get; set; }
        public string? MilkTypeId { get; set; }
        public string? RateTypeId { get; set; }
        public decimal? stdrate { get; set; }
        public string? Description { get; set; }
        public string? RateGenType { get; set; }
        public bool? IsStatus { get; set; }

        public List<MilkPriceDetailModel> PriceDetails { get; set; } = new();
    }
}
