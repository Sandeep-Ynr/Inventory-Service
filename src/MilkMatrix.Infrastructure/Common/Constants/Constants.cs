namespace MilkMatrix.Infrastructure.Common.Constants;

public static partial class Constants
{
    public static class ApprovalSpName
    {
        public const string ApprovalUpsert = "usp_approval_level_insupd"; //For Approval Level Insert/Update
        public const string GetApprovalLevels = "usp_get_approval_level"; //For getting approval levels
        public const string ApprovalDetailsUpsert = "usp_approval_details_insupd"; //For Approval details Insert/Update
        public const string GetApprovalDetails = "usp_get_approval_details"; //For getting approval details
        public const string SetIsApproved = "usp_set_is_approved";
        public const string GetAllowedTablesAndFields = "usp_get_allowed_approval_tablesAndfields";
        public const string GetPageApprovalDetails = "usp_get_page_approval_details";
    }

    public static class RejectionSpName
    {
        public const string RejectionInsert = "usp_rejection_details_insupd"; //For rejction Insert
        public const string GetRejectioDetails = "usp_get_rejection_details"; //For getting rejection details
    }
}
