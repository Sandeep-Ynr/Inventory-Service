
namespace MilkMatrix.Api.Models.Request.Route.RouteContractor
{
    public class RouteContractorUpdateRequestModel
    {
        public int RouteContractorId { get; set; }
        public string? ContractorName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public bool IsStatus { get; set; }
    }
}
