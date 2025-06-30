using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class VillageResponse : CommonLists
    {
            public int TehsilId { get; set; }
            public string TehsilName { get; set; } = string.Empty;
            public string VillageId { get; set; } = String.Empty;
    }
}
