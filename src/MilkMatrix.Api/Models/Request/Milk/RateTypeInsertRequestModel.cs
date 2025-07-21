namespace MilkMatrix.Api.Models.Request.Milk
{
    public class RateTypeInsertRequestModel
    {
        public string? RateTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
