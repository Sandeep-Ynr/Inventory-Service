namespace MilkMatrix.Milk.Models.Request.Plant
{
    public class PlantInsertRequest
    {
        //public int PlantId { get; set; }
        public string? PlantName { get; set; }
        public int CompanyId { get; set; }
        public string? Capacity { get; set; }
        public string? FSSSINumber { get; set; }
        public string? Description { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TehsilId { get; set; }
        public int VillageId { get; set; }
        public int HamletId { get; set; }
        public string? RegionalName { get; set; }
        public string? ContactPerson { get; set; }
        public string? RegionalContactPerson { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
    }
}
