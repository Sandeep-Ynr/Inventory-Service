
using System;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerStagingCollection;

namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollResponse 
    {
        public long id { get; set; }                 // H.HeaderId as Id
        public DateTime? DumpDate { get; set; }      // H.DumpDate
        public string? DumpTime { get; set; }        // H.DumpTime
        public string? shift { get; set; }           // H.Shift
        public string? BatchNo { get; set; }         // H.BatchNo
        public int? MppID { get; set; }              // H.MppID
        public int? BmcID { get; set; }              // H.BmcID
        public string? InsertMode { get; set; }      // H.InsertMode
        public string? Status { get; set; }          // H.Status
        public bool? IsValidated { get; set; }       // H.IsValidated
        public bool? IsProcess { get; set; }         // H.IsProcess
        public DateTime? ProcessDate { get; set; }   // H.ProcessDate
        public bool? IsStatus { get; set; }          // H.is_status
        public string? CreatedBy { get; set; }       // H.created_by
        public DateTime? CreatedOn { get; set; }     // H.created_on
        public string? ModifyBy { get; set; }        // H.modify_by
        public DateTime? ModifyOn { get; set; }      // H.modify_on
        public List<FarmerCollectionStagingDetailModel>? DetailList { get; set; }
    }



    public class FarmerstagingCollResponse
    {
        public long id { get; set; }                 // H.HeaderId as Id
        public DateTime? DumpDate { get; set; }      // H.DumpDate
        public string? DumpTime { get; set; }        // H.DumpTime
        public string? shift { get; set; }           // H.Shift
        public string? BatchNo { get; set; }         // H.BatchNo
        public int? MppID { get; set; }              // H.MppID
        public int? BmcID { get; set; }              // H.BmcID
        public string? InsertMode { get; set; }      // H.InsertMode
        public string? Status { get; set; }          // H.Status
        public bool? IsValidated { get; set; }       // H.IsValidated
        public bool? IsProcess { get; set; }         // H.IsProcess
        public DateTime? ProcessDate { get; set; }   // H.ProcessDate
        public bool? IsStatus { get; set; }          // H.is_status
        public string? CreatedBy { get; set; }       // H.created_by
        public DateTime? CreatedOn { get; set; }     // H.created_on
        public string? ModifyBy { get; set; }        // H.modify_by
        public DateTime? ModifyOn { get; set; }      // H.modify_on
        public string? DetailList { get; set; }
        
    }
    public class FarmerCollectionStagingDetailModel
    {
        public string? Mobile { get; set; }
        public decimal? Fat { get; set; }
        public decimal? Snf { get; set; }
        public decimal? LR { get; set; }
        public decimal? WeightLiter { get; set; }
        public string? Type { get; set; }          // Cow / Buffalo
        public decimal? Rtpl { get; set; } // RTPL
        public decimal? TotalAmount { get; set; }
        public int? SampleId { get; set; }
        public int? Can { get; set; }
        public long? ReferenceId { get; set; }


        
    }
}
