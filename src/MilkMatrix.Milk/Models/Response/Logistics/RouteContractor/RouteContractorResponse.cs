
using System;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Route.RouteContractor
{
    public class RouteContractorResponse:CommonLists
    {

        public string? ContactNumber { get; set; }
        public string? ContractorAddress { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int BusinessId { get; set; }
    }
}
