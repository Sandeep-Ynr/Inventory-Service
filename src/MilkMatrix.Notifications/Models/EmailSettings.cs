namespace MilkMatrix.Notifications.Models;

public class EmailSettings
{
    public int Id { get; set; }
    public string? Subject { get; set; }
    public string MailFrom { get; set; } = string.Empty;
    public string MailTo { get; set; } = string.Empty;
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public string? BodyLink { get; set; }
    public string? Name { get; set; }
    public string? Title { get; set; }
    public string? Content1 { get; set; }
    public string? Content2 { get; set; }
    public string? Content3 { get; set; }
    public string? Content4 { get; set; }
    public string? Content5 { get; set; }
    public string? Content6 { get; set; }
    public string? Attachment { get; set; }
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string? SmtpUserId { get; set; }
    public string? SmtpPassword { get; set; }
    public string? RedirectUrl { get; set; }
}
