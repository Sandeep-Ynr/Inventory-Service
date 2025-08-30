namespace MilkMatrix.Api.Models.Request.PriceApplicability
{
    public class PriceAppRateforModel
    {
        public long CompanyId { get; set; }          // @company_id
        public DateTime TxDate { get; set; }         // @tx_date
        public int ShiftId { get; set; }             // @shift_id
        public string CattleScope { get; set; }      // @cattle_scope
        public long PlantId { get; set; }            // @plant_id
        public long FarmerId { get; set; }           // @farmer_id
        public string Level { get; set; }            // @level (FARMER | SOCIETY | ROUTE etc.)
        public string ViewType { get; set; }         // @view_type (e.g., TOP_MATCH)
        public decimal Fat { get; set; }             // @fat
        public decimal Snf { get; set; }             // @snf
    }
}
