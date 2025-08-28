namespace MilkMatrix.Milk.Models.Request.Accounts.AccountGroups
{
    public class AccountGroupsRequest
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

    public class AccountHeadsRequest
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long GroupId { get; set; }
        public string LedgerType { get; set; }   // char(1)
        public string CashBankType { get; set; } // char(1)
        public long? CityId { get; set; }
        public string CityText { get; set; }
        public string AlternateCode { get; set; }
        public bool BudgetApplicable { get; set; }
        public bool CostCenterApplicable { get; set; }
        public bool TdsApplicable { get; set; }
        public bool IsActive { get; set; }
        public long? BranchId { get; set; }
        public string Notes { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }


}
