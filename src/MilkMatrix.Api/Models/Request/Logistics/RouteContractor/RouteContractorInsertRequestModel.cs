
namespace MilkMatrix.Api.Models.Request.Route.RouteContractor
{
    public class RouteContractorInsertRequestModel
    {
        public int BusinessId { get; set; }
        public string? ContractorName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public bool IsStatus { get; set; }
    }
}
