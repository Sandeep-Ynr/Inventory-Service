using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Core.Entities.Response.Business;

/// <summary>
/// Represents the details of a business entity in the application.
/// </summary>
public class BusinessDetails
{
    /// <summary>
    /// Gets or sets the unique identifier for the business.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the business.
    /// </summary>
    [GlobalSearch]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the prefix for the business name, which can be used for branding or identification purposes.
    /// </summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the business.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the contact person for the business, which is typically the primary point of contact for business-related inquiries.
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number of the contact person for the business. This is used for direct communication with the contact person.
    /// </summary>
    public string ContactMobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the business. This can be a landline or any other contact number associated with the business.
    /// </summary>
    [GlobalSearch]
    public string PhoneNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address associated with the business. This is used for official communication and notifications.
    /// </summary>
    [GlobalSearch]
    public string EmailId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the business, indicating whether it is currently active or inactive.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the business record. This is useful for tracking who added the business to the system.
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the business record was created. This is important for auditing and tracking changes over time.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who last modified the business record. This helps in identifying who made the most recent changes.
    /// </summary>
    public int ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the business record was last modified. This is crucial for maintaining an accurate history of changes made to the business details.
    /// </summary>
    public DateTime ModifiedOn { get; set; }

    /// <summary>
    /// Gets or sets the website URL of the business. This can be used for online presence and customer engagement.
    /// </summary>
    public string Website { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the business's financial year. This is used to associate the business with a specific financial period.
    /// </summary>
    public string PfNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ESIC (Employee State Insurance Corporation) number for the business. This is important for compliance with employee welfare regulations.
    /// </summary>
    public string EsicNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GST (Goods and Services Tax) number for the business. This is essential for tax compliance and invoicing purposes.
    /// </summary>
    public string GstNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN (Permanent Account Number) of the business. This is a unique identifier issued by the tax authorities and is used for various financial transactions.  
    /// </summary>
    public string PanNo { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the state in which the business is registered. This is used for geographical and regulatory purposes.
    /// </summary>
    public string StateId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the data is locked or not. This property is used to prevent modifications to business details after a certain point, ensuring data integrity and compliance with business rules.
    /// </summary>
    public bool LockData { get; set; }

    /// <summary>
    /// Gets or sets the date before which the business data is locked. This is used to prevent changes to business details after a certain date, ensuring data integrity and compliance.
    /// </summary>
    public DateTime LockBefore { get; set; }

    /// <summary>
    /// Gets or sets the financial year for operational purposes. This is used to track the business's financial activities within a specific fiscal period.
    /// </summary>
    public string OpFinancialYear { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date from which the business operations are considered active. This is important for determining the start of financial activities and reporting.
    /// </summary>
    public DateTime OpFromDate { get; set; }

    /// <summary>
    /// Gets or sets the date until which the business operations are considered active. This is important for determining the end of financial activities and reporting.
    /// </summary>
    public int AccountBussinessId { get; set; }

    /// <summary>
    /// Gets or sets the branch sequence character for the business. This is used to identify and differentiate branches within the business, especially in multi-branch setups.
    /// </summary>
    public string BranchSequence { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the logo image associated with the business. This is used to display the business's logo in various interfaces and reports.
    /// </summary>
    public string? LogoImageId { get; set; }

    /// <summary>
    /// Gets or sets the file path for the logo image associated with the business. This is used to store and retrieve the logo image for display purposes.
    /// </summary>
    public string? LogoImagePath { get; set; }

    /// <summary>
    /// Gets or sets the branch code for the business. This is used to uniquely identify branches within the business, especially in multi-branch setups.
    /// </summary>
    public string BranchCode { get; set; }
}
