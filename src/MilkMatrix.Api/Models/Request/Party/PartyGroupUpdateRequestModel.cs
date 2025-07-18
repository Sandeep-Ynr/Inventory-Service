namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyGroupUpdateRequestModel
    {
        public long GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string? GroupShortName { get; set; }
        public bool IsStatus { get; set; }
        public long? ModifyBy { get; set; }
    }
}
