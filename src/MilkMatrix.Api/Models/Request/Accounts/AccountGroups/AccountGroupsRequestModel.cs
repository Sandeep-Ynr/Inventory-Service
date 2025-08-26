namespace MilkMatrix.Api.Models.Request.Accounts.Accountgroups
{
    public class AccountGroupsRequestModel
    {
        public long Id { get; set; }

        public long BusinessId { get; set; }   // FK -> tbl_business

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public long? ParentId { get; set; }    // Self FK

        public int ScheduleId { get; set; }    // FK -> tbl_status.status_id

        public bool IsActive { get; set; } = true;

        public bool AllowPosting { get; set; } = false;

        public string? Notes { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public long? ModifyBy { get; set; }

        public DateTime? ModifyOn { get; set; }

    }

    
}
