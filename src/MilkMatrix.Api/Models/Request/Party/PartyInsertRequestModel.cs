namespace MilkMatrix.Api.Models.Request.Party
{
    public class PartyInsertRequestModel
    {
        public string PartyCode { get; set; } = string.Empty;
        public int GroupId { get; set; } = 0;
        public string PartyName { get; set; } = string.Empty;
        public string? PartyEmail { get; set; }
        public string? PartyShortName { get; set; }
        public string? PartyAddress { get; set; }
        public string? PartyPinCode { get; set; }
        public string? PartyPhoneNo { get; set; }
        public string? PartyLicenceNo { get; set; }
        public string? PartyGstNo { get; set; }
        public string? PartyOwnerName { get; set; }
        public string? PartyOwnerEmail { get; set; }
        public string? PartyOwnerPhoneNo { get; set; }
        public bool IsStatus { get; set; }
        public long? CreatedBy { get; set; }
    }
}
