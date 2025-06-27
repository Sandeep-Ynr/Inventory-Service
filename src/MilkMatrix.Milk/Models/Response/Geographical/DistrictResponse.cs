using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
    public class DistrictResponse : CommonLists
    {
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
        public string DistrictId { get; set; } = string.Empty;

    }
}

