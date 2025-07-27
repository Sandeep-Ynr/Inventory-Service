namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class PriceAppInsertRequestModel
    {
        public int? BusinessEntityId { get; set; }
        public string? RateCode { get; set; }
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public int? ShiftId { get; set; }
        public string? RateFor { get; set; }
        public bool? IsStatus { get; set; }
    }
}
