using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Member.MemberMilkProfile
{
    public class MemberMilkProfileResponse : CommonLists
    {
        public long MilkProfileID { get; set; }
        public long MemberID { get; set; }
        public string AnimalType { get; set; }
        public int NoOfMilchAnimals { get; set; }
        public decimal? AvgMilkYield { get; set; }
        public string PreferredShift { get; set; }
        public DateOnly PouringStartDate { get; set; }
        public bool is_status { get; set; }
        public DateTime created_on { get; set; }
        public long created_by { get; set; }
        public DateTime? modify_on { get; set; }
        public long? modify_by { get; set; }
        public bool is_deleted { get; set; }
    }
}
