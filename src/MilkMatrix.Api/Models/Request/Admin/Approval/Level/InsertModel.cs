namespace MilkMatrix.Api.Models.Request.Admin.Approval.Level
{
    public class InsertModel
    {
        public int UserId { get; set; }

        public int? PageId { get; set; }

        public int Level { get; set; }

        public int? BusinessId { get; set; }

        public decimal? Amount { get; set; }

        public int? DepartmentId { get; set; }
    }
}
