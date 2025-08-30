namespace MilkMatrix.Api.Models.Request.Accounts.Accountgroups
{
    public class AccountGroupsUpdateRequestModel
    {
    
        public long BusinessId { get; set; }   // FK -> tbl_business

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public long? ParentId { get; set; }    // Self FK

        public int ScheduleId { get; set; }    // FK -> tbl_status.status_id

        public bool IsActive { get; set; } = true;

        public bool AllowPosting { get; set; } = false;

        public string? Notes { get; set; }

        public int? ModifyBy { get; set; }



    }

    public class AccountHeadsUpdateRequestModel
    {
        public long BusinessId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long GroupId { get; set; }
        public string LedgerType { get; set; }   // char(1)
        public string CashBankType { get; set; } // char(1)
        public long? CityId { get; set; }
        public string CityText { get; set; }
        public string? AlternateCode { get; set; }
        public bool BudgetApplicable { get; set; }
        public bool CostCenterApplicable { get; set; }
        public bool TdsApplicable { get; set; }
        public bool IsActive { get; set; }
        public long? BranchId { get; set; }
        public string Notes { get; set; }
        public long? ModifyBy { get; set; }
        
    }


}
