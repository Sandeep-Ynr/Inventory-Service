
namespace MilkMatrix.Api.Models.Request.Route.RouteContractor
{
    public class RouteContractorUpdateRequestModel
    {
        public int RouteContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public bool IsStatus { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
