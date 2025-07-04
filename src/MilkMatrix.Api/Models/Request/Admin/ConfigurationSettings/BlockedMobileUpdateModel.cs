using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;

public class BlockedMobileUpdateModel
{
    [Required]
    public int Id { get; set; }

    public string? MobileNumber { get; set; }

    public string? ContactName { get; set; }

    public int? BusinessId { get; set; }

    public bool? IsActive { get; set; }
}
