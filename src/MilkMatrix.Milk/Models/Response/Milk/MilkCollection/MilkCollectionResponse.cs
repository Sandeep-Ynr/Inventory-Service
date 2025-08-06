
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Api.Models.Request.MilkCollection
{
    public class MilkCollectionResponse: CommonLists
    {
        public string? BusinessID { get; set; }
        public long? MemberId { get; set; }
        public string? CenterType { get; set; }
        public int? CenterId { get; set; }
        public int? RouteId { get; set; }
        public DateTime? CollectionDate { get; set; }
        public string? Shift { get; set; }
        public string? MilkType { get; set; }
        public decimal? QuantityLtr { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? RatePerLtr { get; set; }
        public decimal? Amount { get; set; }
        public string? CollectionMode { get; set; }
        public string? Status { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
