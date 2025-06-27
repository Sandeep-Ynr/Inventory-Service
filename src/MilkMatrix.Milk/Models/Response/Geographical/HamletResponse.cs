using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class HamletResponse : CommonLists
    {
        public int VillageId { get; set; }
        public string VillageName { get; set; } = string.Empty;
    }
}
