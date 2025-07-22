using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings.CommonStatus
{
    public class StatusInsertModel
    {
        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the status, which can be used to categorize or group statuses.
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this status is active.
        /// </summary>
        public bool? ShowToUser { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent status, if applicable.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the abbreviation for the status, which can be used for display purposes.
        /// </summary>
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the business associated with this status.
        /// </summary>
        [Required]
        public int BusinessId { get; set; }

    }
}
