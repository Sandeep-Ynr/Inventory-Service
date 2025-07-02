using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;

public class BlockedMobileInsertModel
{

    [Required(AllowEmptyStrings = false)]
    public string MobileNumber { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public int? BusinessId { get; set; }
}
