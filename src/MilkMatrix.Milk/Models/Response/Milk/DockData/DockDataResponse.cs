using System;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Milk.DockData
{
    public class DockDataResponse: CommonLists
    {
        public string? BmcCode { get; set; }
        public DateTime DumpDate { get; set; }
        public string? Shift { get; set; }
        public string? UpdateStatus { get; set; }
        public int UpdatedRecords { get; set; }
        public bool? is_status { get; set; }
        public bool? is_deleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
