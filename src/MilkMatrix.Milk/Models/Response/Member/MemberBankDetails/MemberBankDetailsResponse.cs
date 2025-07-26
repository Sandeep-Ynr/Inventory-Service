using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member.MemberBankDetails
{
    public class MemberBankDetailsResponse : CommonLists
    {
        
        public string? MemberCode { get; set; }          
        public int BankID { get; set; }                  
        public string? BankName { get; set; }            
        public int BranchID { get; set; }                
        public string? BranchName { get; set; }          
        public string? AccountHolderName { get; set; }   
        public string? AccountNumber { get; set; }       
        public string? IFSCCode { get; set; }            
        public bool? IsJointAccount { get; set; }        
        public string? PassbookFilePath { get; set; }    
        public DateTime CreatedOn { get; set; }          
        public long CreatedBy { get; set; }              
        public DateTime? ModifyOn { get; set; }          
        public long? ModifyBy { get; set; }
    }
}
