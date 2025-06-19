namespace MilkMatrix.Api.Models.Request.Admin.Role
{
    public class RoleUpsertModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? BusinessId { get; set; }

        public bool? Status { get; set; }
    }
}
