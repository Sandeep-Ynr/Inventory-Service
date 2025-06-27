using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response.Geographical
{
public class TehsilResponse : CommonLists
{
        public int DistrictId { get; set; }
        //public string DistrictName { get; set; } = string.Empty;
        public string TehsilId { get; set; } = string.Empty;

    }
}
