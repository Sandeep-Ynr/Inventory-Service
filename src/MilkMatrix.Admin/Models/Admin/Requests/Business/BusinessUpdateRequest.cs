namespace MilkMatrix.Admin.Models.Admin.Requests.Business;

/// <summary>
/// Represents a request model for updating business details in the application.
/// </summary>
public class BusinessUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the business. This is used to identify the specific business record that needs to be updated.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the business.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the branchCode for the business name, which can be used for branding or identification purposes.
    /// </summary>
    public string? BranchCode { get; set; } 

    /// <summary>
    /// Gets or sets the prefix for the business name, which can be used for branding or identification purposes.
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Gets or sets the address of the business.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the contact person for the business, which is typically the primary point of contact for business-related inquiries.
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Gets or sets the mobile number of the contact person for the business. This is used for direct communication with the contact person.
    /// </summary>
    public string? ContactMobile { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the business. This can be a landline or any other contact number associated with the business.
    /// </summary>
    public string? PhoneNo { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the business. This is used for official communication and notifications.
    /// </summary>
    public string? EmailId { get; set; }

    /// <summary>
    /// Gets or sets the status of the business, indicating whether it is currently active or inactive.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who modified the business record. This is useful for tracking who added the business to the system.
    /// </summary>
    public int ModifyBy { get; set; }

    /// <summary>
    /// Gets or sets the website URL of the business. This can be used for online presence and customer engagement.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the business's financial year. This is used to associate the business with a specific financial period.
    /// </summary>
    public string? PfNo { get; set; }

    /// <summary>
    /// Gets or sets the ESIC (Employee State Insurance Corporation) number for the business. This is important for compliance with employee welfare regulations.
    /// </summary>
    public string? EsicNo { get; set; }

    /// <summary>
    /// Gets or sets the GST (Goods and Services Tax) number for the business. This is essential for tax compliance and invoicing purposes.
    /// </summary>
    public string? GstNo { get; set; }

    /// <summary>
    /// Gets or sets the PAN (Permanent Account Number) of the business. This is a unique identifier issued by the tax authorities and is used for various financial transactions.  
    /// </summary>
    public string? PanNo { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the state in which the business is registered. This is used for geographical and regulatory purposes.
    /// </summary>
    public int? StateId { get; set; }

    /// <summary>
    /// Gets or sets whether the data is locked or not. This property is used to prevent modifications to business details after a certain point, ensuring data integrity and compliance with business rules.
    /// </summary>
    public bool? LockData { get; set; }

    /// <summary>
    /// Gets or sets the date before which the business data is locked. This is used to prevent changes to business details after a certain date, ensuring data integrity and compliance.
    /// </summary>
    public DateTime? LockBefore { get; set; }

    /// <summary>
    /// Gets or sets the financial year for operational purposes. This is used to track the business's financial activities within a specific fiscal period.
    /// </summary>
    public string? OpFinancialYear { get; set; }

    /// <summary>
    /// Gets or sets the date from which the business operations are considered active. This is important for determining the start of financial activities and reporting.
    /// </summary>
    public DateTime? OpFromDate { get; set; }

    /// <summary>
    /// Gets or sets the date until which the business operations are considered active. This is important for determining the end of financial activities and reporting.
    /// </summary>
    public int? AccountBussinessId { get; set; }

    /// <summary>
    /// Gets or sets the branch sequence character for the business. This is used to identify and differentiate branches within the business, especially in multi-branch setups.
    /// </summary>
    public string? BranchSequence { get; set; }

    /// <summary>
    /// Gets or sets logo image identifier for the business. This is used to associate a visual representation (logo) with the business entity, enhancing branding and recognition.
    /// </summary>
    public string? LogoImageId { get; set; }
}
