namespace MilkMatrix.Admin.Models.Admin.Responses.User;

public class UserApprovalNotification
{
    public int DocumentId { get; set; }

    public string PageName { get; set; }

    public string PageUrl { get; set; }

    public string TableName { get; set; }

    public int PageId { get; set; }

    public int BusinessId { get; set; }
}
