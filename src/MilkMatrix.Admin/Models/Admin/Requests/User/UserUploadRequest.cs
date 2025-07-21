using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Admin.Models.Admin.Requests.User;

public class UserUploadRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;

    public string HrmsCode { get; set; } = string.Empty;

    public string RoleId { get; set; } = string.Empty;

    public string BusinessId { get; set; } = string.Empty;

    public string ReportingId { get; set; } = string.Empty;

    public int? UserType { get; set; }

    public string MobileNumber { get; set; } = string.Empty;

    public int CreatedBy { get; set; }

    public string? IsMFA { get; set; } = "N";

    public string? IsBulkUser { get; set; } = "Y";

    public string? ChangePassword { get; set; } = "Y";

    public bool IsActive { get; set; } = true;

    public string? ProcessStatus { get; set; } = "Pending";

    public string? ErrorMessage { get; set; }
}
