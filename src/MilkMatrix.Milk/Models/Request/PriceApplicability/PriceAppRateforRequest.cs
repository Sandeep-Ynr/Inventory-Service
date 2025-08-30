namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class PriceAppRateforRequest
    {
        public long CompanyId { get; set; }          // @company_id
        public DateTime TxDate { get; set; }         // @tx_date
        public int ShiftId { get; set; }             // @shift_id
        public string CattleScope { get; set; }      // @cattle_scope
        public decimal Fat { get; set; }             // @fat
        public decimal Snf { get; set; }             // @snf
        public string ViewType { get; set; }         // @view_type (e.g., TOP_MATCH)
        public string Level { get; set; }            // @level (FARMER | SOCIETY | ROUTE etc.)
        public long? PlantId { get; set; }            // @plant_id
        public long? FarmerId { get; set; }           // @farmer_id
        public long? SocietyId { get; set; }
        public long? RouteId { get; set; }
        public long? BmcId { get; set; }
        public long? MccId { get; set; }
    }
}
