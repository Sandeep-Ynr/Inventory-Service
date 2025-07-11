namespace MilkMatrix.Milk.Models.Queries;

public static partial class BankQueries
{
    public static class BankRelgionQueries
    {
        public const string GetBankRelgion = "usp_regionalbank_details";
        public const string AddBankRelgion = "usp_regionalbank_insupd";
        public const string GetBankRelgionList = "usp_regional_bank_list";

        
    }
    public static class BankTypeQueries
    {
        public const string GetBankType = "usp_banktype_details";
        public const string AddBankType = "usp_banktype_insupd";
        public const string GetBankTypeList = "usp_bank_type_list";

    }
    public static class BankMasterQueries
    {
        public const string GetBank = "usp_bank_details";
        public const string AddBank = "usp_bank_insupd";
        public const string GetBankList = "usp_bank_list";
    }

    public static class BranchQueries
    {
        public const string AddBranch = "usp_branch_insupd";
        public const string GetBranchList = "usp_branch_list";
    }

}
