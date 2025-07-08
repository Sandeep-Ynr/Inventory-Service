namespace MilkMatrix.Milk.Models.Queries;

public static partial class BankQueries
{
    public static class BankRelgionQueries
    {
        public const string GetBankRelgion = "usp_regionalbank_details";
        public const string AddBankRelgion = "usp_regionalbank_insupd";
    }
    public static class BankTypeQueries
    {
        public const string GetBankType = "usp_banktype_details";
        public const string AddBankType = "usp_banktype_insupd";
    }
}
