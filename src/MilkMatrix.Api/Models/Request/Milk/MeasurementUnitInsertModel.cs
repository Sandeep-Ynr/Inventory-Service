namespace MilkMatrix.Api.Models.Request.Milk
{
    public class MeasurementUnitInsertModel
    {
         public string? MeasurementUnitCode { get; set; }
        public string? MeasurementUnitName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
