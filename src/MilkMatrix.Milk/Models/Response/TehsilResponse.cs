using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response
{
public class TehsilResponse : CommonLists
{
        public int DistrictId { get; set; }
        public string DistrictName { get; set; } = String.Empty;
        public string TehsilId { get; set; } = String.Empty;

    }
}
