namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings
{
    /// <summary>
    /// Represents the model for inserting SMS control settings in the administration panel.
    /// </summary>
    public class SmsControlInsertModel
    {
        /// <summary>
        /// Gets or sets the SMS merchant name.
        /// </summary>
        public string SmsMerchant { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the sender ID for SMS.
        /// </summary>
        public string SenderId { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the authentication key for SMS.
        /// </summary>
        public string AuthKey { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the URL link for SMS service.
        /// </summary>
        public string UrlLink { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the template ID for SMS.
        /// </summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// Gets or sets the order ID associated with the SMS control.
        /// </summary>
        public int OrderId { get; set; }
    }
}
