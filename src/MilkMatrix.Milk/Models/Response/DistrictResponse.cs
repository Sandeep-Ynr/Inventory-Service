using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response
{
    public class DistrictResponse : CommonLists
    {
        public int StateId { get; set; }
        public string StateName { get; set; } = String.Empty;
        public string DistrictId { get; set; } = String.Empty;

    }
}

