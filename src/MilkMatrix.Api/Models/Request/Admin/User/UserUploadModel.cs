using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Admin.User
{
    public class UserUploadModel
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }

        public string? HrmsCode { get; set; }

        public string RoleId { get; set; }

        public string BusinessId { get; set; }

        public string ReportingId { get; set; }
        public int UserType { get; set; }

        public string MobileNumber { get; set; }
    }
}
