namespace MilkMatrix.Api.Models.Request.Geographical.Tehsil
{
    public class TehsilInsertRequestModel
    {
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }
        public int DistrictId { get; set; }
        public bool? IsStatus { get; set; }
    }
}
