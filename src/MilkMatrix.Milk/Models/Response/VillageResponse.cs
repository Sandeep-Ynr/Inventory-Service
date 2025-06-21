using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response
{
    public class VillageResponse : CommonLists
    {
            public int TehsilId { get; set; }
            public string TehsilName { get; set; } = String.Empty;
            //public string VillageId { get; set; } = String.Empty;
    }
}
