using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class VillageResponse : CommonLists
    {

        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        public int? CreatedBy { get; set; }
        
    }
}
