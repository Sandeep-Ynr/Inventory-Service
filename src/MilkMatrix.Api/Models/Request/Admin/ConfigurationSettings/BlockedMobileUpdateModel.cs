using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;

public class BlockedMobileUpdateModel
{
    [Required]
    public int Id { get; set; }

    public string MobileNumber { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public int? BusinessId { get; set; }

    public bool? IsActive { get; set; }
}
