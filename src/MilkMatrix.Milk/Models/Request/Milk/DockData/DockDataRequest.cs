using System;

namespace MilkMatrix.Milk.Models.Request.Milk.DockData
{
    public class DockDataRequest
    {
        public int BusinessId { get; set; }
        public int DockDataUpdateId { get; set; }
        public string BmcId { get; set; }
        public DateTime DumpDate { get; set; }
        public string Shift { get; set; }
        public string UpdateStatus { get; set; }
        public int UpdatedRecords { get; set; }
        public string Remarks { get; set; }
        public bool? IsStatus { get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
