namespace MilkMatrix.Core.Entities.Response.Business;

public class BusinessMaster
{
    public int Id { get; set; }
    public string BranchAlias { get; set; } = string.Empty;
    public int? AccountBusinessId { get; set; }
    public int? UserId { get; set; }
}
