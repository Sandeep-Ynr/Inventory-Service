using MilkMatrix.Milk.Models.Request.PriceApplicability;

namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class PriceAppUpdateRequestModel
    {
        public int? mappingid { get; set; }
        public string? RvOriginal { get; set; }
        public int? BusinessEntityId { get; set; }
        public int? RateCodeId { get; set; }
        public string? ModuleCode { get; set; }
        public string? ModuleName { get; set; }
        public DateTime? WithEffectDate { get; set; }
        public int? ShiftId { get; set; }
        public string? RateFor { get; set; }
        public bool? IsStatus { get; set; }
        public int? Priority { get; set; }
        public string? cattleScope { get; set; }
        public string? applied_for { get; set; }
        public string? applied_shift_scope { get; set; }
        public List<RateMappingTarget>? Targets { get; set; }
    }
}
