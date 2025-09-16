using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Models.Request.Milk.Transactions.Dispatch
{
    public class DispatchUpdateRequest
    {
        public decimal? RowId { get; set; }               
        public string? BusinessEntityId { get; set; }    
        public long? PlantId { get; set; }                
        public long? MccId { get; set; }                  
        public string? BMC_Other_Code { get; set; }      
        public string? MPP_Other_Code { get; set; }      
        public long? CntCode { get; set; }                
        public long? SocCode { get; set; }                
        public long? RouteId { get; set; }
        public string? ShiftId { get; set; }
        public DateTime? DispatchDate { get; set; }
        public DateTime? DispatchTime { get; set; }
        public int? TotalSamples { get; set; }
        public string? TypeId { get; set; }
        public string? Grade { get; set; }               
        public decimal? Weight { get; set; }              
        public decimal? WeightLiter { get; set; }         
        public decimal? Fat { get; set; }                 
        public decimal? Snf { get; set; }                 
        public decimal? Lr { get; set; }                  
        public decimal? Protein { get; set; }             
        public decimal? Water { get; set; }               
        public decimal? Rtpl { get; set; }                
        public decimal? TotalAmount { get; set; }         
        public int? Can { get; set; }                     
        public bool? IsQtyAuto { get; set; }              
        public bool? IsQltyAuto { get; set; }             
        public DateTime? QtyTime { get; set; }            
        public DateTime? QltyTime { get; set; }           
        public decimal? KgLtrConst { get; set; }          
        public decimal? LtrKgConst { get; set; }          
        public string? QtyMode { get; set; }             
        public string? Remark { get; set; }              
        public string? DeviceId { get; set; }            
        public long? AnalyzerCode { get; set; }           
        public string? AnalyzerString { get; set; }      
        public long? CUserId { get; set; }                
        public DateTime? CDateTime { get; set; }          
        public long? MUserId { get; set; }                
        public DateTime? MDateTime { get; set; }          
        public bool? IsApproved { get; set; }             
        public bool? IsRejected { get; set; }             
        public bool? IsDelete { get; set; }               
        public string? PublicIp { get; set; }            
        public DateTime? LastSynchronized { get; set; }   
        public string? SyncStatus { get; set; }          
        public DateTime? SyncTime { get; set; }           
        public string? Batch_Id { get; set; }            
        public string? InsertMode { get; set; }          
        public bool? Is_Status { get; set; }             
        public int? Created_By { get; set; }             
        public DateTime? Created_On { get; set; }        
        public int? Modify_By { get; set; }              
        public DateTime? Modify_On { get; set; }         
    }
}
