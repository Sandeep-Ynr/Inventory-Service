using System;

namespace MilkMatrix.Api.Models.Request.Milk.DockData
{
    public class DockDataUpdateRequestModel
    {
        public int DockDataUpdateId { get; set; }
        public string? BmcId { get; set; }
        public DateTime DumpDate { get; set; }
        public string? Shift { get; set; }
        public string? UpdateStatus { get; set; }
        public int UpdatedRecords { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }

    }
}
