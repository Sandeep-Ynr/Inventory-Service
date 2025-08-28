using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Milk.Models.Response.Accounts.AccountGroups
{
    public class AccountGroupsResponse 
    {
        public int Id { get; set; }

        public int Business_Id { get; set; }   // FK -> tbl_business

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int? Parent_Id { get; set; }    // Self FK

        public string Parent_Name { get; set; }   // ✅ Parent Group Name

        public int? Root_Id { get; set; }      // Root ancestor

        public int Level_No { get; set; }

        public int Schedule_Id { get; set; }    // FK -> tbl_status.status_id

        public string? Path { get; set; }      // Materialized path

        public int? Lft { get; set; }          // Nested set left

        public int? Rgt { get; set; }          // Nested set right

        public bool Is_Active { get; set; } = true;

        public bool Allow_Posting { get; set; } = false;

        public string? Notes { get; set; }

        public int? Created_By { get; set; }

        public DateTime Created_On { get; set; } 

        public int? Modify_By { get; set; }

        public DateTime? Modify_On { get; set; }

        public bool Is_Deleted { get; set; }



    }

    public class AccountHeadsResponse
    {
        public long id { get; set; }
        public long business_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public long group_id { get; set; }
        public string group_name { get; set; }   // from join
        public string ledger_type { get; set; }
        public string cash_bank_type { get; set; }
        public long? city_id { get; set; }
        public string city_text { get; set; }
        public string alternate_code { get; set; }
        public bool budget_applicable { get; set; }
        public bool cost_center_applicable { get; set; }
        public bool tds_applicable { get; set; }
        public bool is_active { get; set; }
        public long? branch_id { get; set; }
        public string notes { get; set; }
        public long created_by { get; set; }
        public DateTime created_on { get; set; }
        public long? modify_by { get; set; }
        public DateTime? modify_on { get; set; }
        public bool is_deleted { get; set; }

    }


}
