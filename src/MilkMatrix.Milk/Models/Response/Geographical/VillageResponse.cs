using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class VillageResponse : CommonLists
    {
        public string? BusinessId { get; set; }
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }
    }
}
