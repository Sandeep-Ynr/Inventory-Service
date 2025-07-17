namespace MilkMatrix.Api.Models.Request.Animal
{
    public class AnimalTypeInsertRequestModel
    {
        public string? AnimalTypeCode { get; set; }
        public string? AnimalTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsStatus { get; set; }
    }
}
