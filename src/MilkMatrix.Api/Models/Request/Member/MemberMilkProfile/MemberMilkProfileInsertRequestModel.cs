using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberMilkProfile
{
    public class MemberMilkProfileInsertRequestModel
    {
        public long MemberID { get; set; }
        public string AnimalType { get; set; }
        public int NoOfMilchAnimals { get; set; }
        public decimal? AvgMilkYield { get; set; }
        public string PreferredShift { get; set; }
        public DateOnly PouringStartDate { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
