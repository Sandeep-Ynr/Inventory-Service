namespace MilkMatrix.Milk.Models.Request.PriceApplicability
{
    public class RateMappingTarget
    {
        public long? PlantId { get; set; }
        public long? MccId { get; set; }
        public long? BmcId { get; set; }
        public long? RouteId { get; set; }
        public long? SocietyId { get; set; }
        public long? FarmerId { get; set; }
        public bool ApplyToAllBelow { get; set; }
    }
}
