namespace MilkMatrix.Notifications.Models.OTP.Response;

public class SMSConfiguration
{
    public int Id { get; set; }
    public string? SmsFor { get; set; }
    public string? SmsBody { get; set; }
    public int BusinessId { get; set; }
    public string? BusinessName { get; set; }
    public int CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? CreatedOn { get; set; }
    public int UpdatedBy { get; set; }
    public string? UpdatedByName { get; set; }
    public string? UpdatedOn { get; set; }
    public bool IsStatus { get; set; }
}
