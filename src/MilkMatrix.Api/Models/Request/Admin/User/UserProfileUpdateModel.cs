namespace MilkMatrix.Api.Models.Request.Admin.User
{
    public class UserProfileUpdateModel
    {
        public string? UserName { get; set; }

        public string? Mobile { get; set; }
        public int? ImageId { get; set; } = 0;
    }
}
