namespace MilkMatrix.Admin.Models.Admin.Responses.Rejection;

/// <summary>
/// Represents the details of a rejection.
/// </summary>
public class Details
{
    /// <summary>
    /// The unique identifier for the rejection.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// The unique identifier for the page associated with the rejection.
    /// </summary>
    public int PageId { get; set; }

    /// <summary>
    /// The unique identifier for the business associated with the rejection.
    /// </summary>
    public int BusinessId { get; set; }

    public string PageName { get; set; }

    public string BusinessName { get; set; }

    /// <summary>
    /// The level of the rejection, indicating its severity or importance.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// The reason for the rejection, providing context or explanation for the decision.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// The document number associated with the rejection, if applicable.
    /// </summary>
    public string? DocNo { get; set; }

    /// <summary>
    /// The name of the person who rejected the item or request.
    /// </summary>
    public string? RejectedBy { get; set; }

    /// <summary>
    /// The date and time when the rejection was created.
    /// </summary>
    public DateTime? RejectedAt { get; set; }
}
