namespace MilkMatrix.Api.Models.Request.Accounts.HSN
{
    public class HSNUpdateRequestModel
    {
      
        public int BusinessId { get; set; }
        public string HSNCode { get; set; }
        public string Description { get; set; }

        public decimal? IgstRate { get; set; }
        public decimal? CgstRate { get; set; }
        public decimal? SgstRate { get; set; }
        public decimal? CessRate { get; set; }

        public long? CgstInputLedgerId { get; set; }
        public long? CgstOutputLedgerId { get; set; }
        public long? SgstInputLedgerId { get; set; }
        public long? SgstOutputLedgerId { get; set; }
        public long? IgstInputLedgerId { get; set; }
        public long? IgstOutputLedgerId { get; set; }
        public long? CessInputLedgerId { get; set; }
        public long? CessOutputLedgerId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? WefDate { get; set; } 
    }
}
