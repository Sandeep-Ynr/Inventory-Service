using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.District
{
    public class DistrictUpdateRequestModel
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? StateId { get; set; }
        public bool? IsStatus { get; set; }
        
    }
}
