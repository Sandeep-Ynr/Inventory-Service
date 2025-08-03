using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Hamlet
{
    public class HamletInsertRequestModel
    {
        public string? BusinessId { get; set; }
        public string? HamletName { get; set; }
        public int? VillageId { get; set; }
        public bool? IsStatus { get; set; }
    }
}
