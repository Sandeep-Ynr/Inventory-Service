namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;

public class BlockedMobilesUpdate
{
    public int Id { get; set; }
    public string? MobileNumber { get; set; }

    public string? ContactName { get; set; }

    public int? BusinessId { get; set; }

    public bool? IsActive { get; set; }

    public int ModifyBy { get; set; }
}
