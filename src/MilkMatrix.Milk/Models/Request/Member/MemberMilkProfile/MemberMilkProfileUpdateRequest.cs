using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile
{
    public class MemberMilkProfileUpdateRequest
    {
        public long MilkProfileID { get; set; }
        public long MemberID { get; set; }
        public string? AnimalTypeID { get; set; }
        public int NoOfMilchAnimals { get; set; }
        public decimal? AvgMilkYield { get; set; }
        public string? PreferredShift { get; set; }
        public DateOnly PouringStartDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
