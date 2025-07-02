namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;

public class BlockedMobilesInsert
{
    public string MobileNumber { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public int? BusinessId { get; set; }

    public int CreatedBy { get; set; }
}
