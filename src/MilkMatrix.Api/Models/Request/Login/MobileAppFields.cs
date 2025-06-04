namespace MilkMatrix.Api.Models.Request.Login
{
    public class MobileAppFields
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? FireBaseToken { get; set; }
        public string? AppVersion { get; set; }
        public string? MobileOs { get; set; }
        public string? MobileMake { get; set; }
        public string? MobileModel { get; set; }
    }
}
