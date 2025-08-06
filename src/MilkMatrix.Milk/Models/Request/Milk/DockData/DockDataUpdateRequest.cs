using System;

namespace MilkMatrix.Milk.Models.Request.Milk.DockData
{
    public class DockDataUpdateRequest
    {
        public int BusinessId { get; set; }
        public string? BmcId { get; set; }
        public DateTime DumpDate { get; set; }
        public string? Shift { get; set; }
        public string? UpdateStatus { get; set; }
        public int UpdatedRecords { get; set; }
        public string? Remarks { get; set; }
        public bool? IsStatus { get; set; }

    }
}
