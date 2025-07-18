namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyGroupInsertRequestModel
    {
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string? GroupShortName { get; set; }
        public bool IsStatus { get; set; }
    }
}

