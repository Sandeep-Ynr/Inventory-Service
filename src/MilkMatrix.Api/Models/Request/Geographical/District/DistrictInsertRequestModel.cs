namespace MilkMatrix.Api.Models.Request.Geographical.District
{
    public class DistrictInsertRequestModel
    {
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? StateId { get; set; }
        public bool? IsStatus { get; set; }
    }
}
