
using System;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerStagingCollection;

namespace MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerCollResponse:CommonLists
    {
        public long Id { get; set; }                 // H.HeaderId as Id
        public DateTime? DumpDate { get; set; }      // H.DumpDate
        public string? DumpTime { get; set; }        // H.DumpTime
        public string? Shift { get; set; }           // H.Shift
        public string? BatchNo { get; set; }         // H.BatchNo
        public int? MppID { get; set; }              // H.MppID
        public int? BmcID { get; set; }              // H.BmcID
        public string? FarmerId { get; set; }
        public string? InsertMode { get; set; }      // H.InsertMode
        public string? Status { get; set; }          // H.Status
        public int? CompanyCode { get; set; }        // H.CompanyCode
        public string? Name { get; set; }            // H.IMEI_No as name
        public bool? IsValidated { get; set; }       // H.IsValidated
        public bool? IsProcess { get; set; }         // H.IsProcess
        public DateTime? ProcessDate { get; set; }   // H.ProcessDate
        public bool? IsStatus { get; set; }          // H.is_status
        public bool? IsDeleted { get; set; }         // H.is_deleted
        public string? CreatedBy { get; set; }       // H.created_by
        public DateTime? CreatedOn { get; set; }     // H.created_on
        public string? ModifyBy { get; set; }        // H.modify_by
        public DateTime? ModifyOn { get; set; }      // H.modify_on
        public string? BusinessId { get; set; }
        public string? DetailList { get; set; }


    }
}
