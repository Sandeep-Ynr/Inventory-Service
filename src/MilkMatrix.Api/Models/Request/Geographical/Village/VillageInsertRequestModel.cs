using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageInsertRequestModel
    {
        public string? BusinessId { get; set; }
        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsStatus { get; set; }
        public int? CreatedBy { get; set; }
        
    }
}
