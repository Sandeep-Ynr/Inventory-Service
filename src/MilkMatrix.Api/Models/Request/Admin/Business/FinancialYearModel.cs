namespace MilkMatrix.Api.Models.Request.Admin.Business
{
    /// <summary>
    /// Represents a model for managing financial years in the application.
    /// </summary>
    public class FinancialYearModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the financial year.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the status of the financial year. 
        /// </summary>
        public bool? IsActive { get; set; } = true;
    }
}
