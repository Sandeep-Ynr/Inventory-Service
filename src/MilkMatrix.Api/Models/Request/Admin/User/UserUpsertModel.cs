using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Admin.User
{
    public class UserUpsertModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public string? HrmsCode { get; set; }

        public string? Roles { get; set; }

        public string? BusinessIds { get; set; }

        public string? ReportingId { get; set; }
        public int? UserType { get; set; }

        public int? ImageId { get; set; }
        public string? Mobile { get; set; }

        public bool? Status { get; set; }

        public YesOrNo? IsMFA { get; set; }
    }
}
