using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Member.MemberMilkProfile
{
    public class MemberMilkProfileInsertRequestModel
    {
        public string? AnimalTypeID { get; set; }
        public int NoOfMilchAnimals { get; set; }
        public decimal? AvgMilkYield { get; set; }
        public string? PreferredShift { get; set; }
        public DateOnly PouringStartDate { get; set; }
    }
}
