namespace MilkMatrix.Core.Entities.Response.Business;

public class BusinessMaster
{
    public string Id { get; set; } = string.Empty;
    public string BranchAlias { get; set; } = string.Empty;
    public int? AccountBusinessId { get; set; }
    public int? UserId { get; set; }
}
