using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member.MemberMilkProfile
{
    public class MemberMilkProfileResponse : CommonLists
    {
        public long MemberID { get; set; }               
        public string? MemberCode { get; set; }          
        public string? AnimalTypeID { get; set; }        
        public string? AnimalTypeName { get; set; }      
        public int NoOfMilchAnimals { get; set; }        
        public decimal? AvgMilkYield { get; set; }       
        public string? PreferredShift { get; set; }      
        public DateTime PouringStartDate { get; set; }   
        
    }
}
