namespace MilkMatrix.Api.Models.Request.Admin.Rejection
{
    /// <summary>
    /// Represents a request to insert a rejection into the system.
    /// </summary>
    public class RejectionModel
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

        /// <summary>
        /// The level of the rejection, indicating its severity or importance.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The reason for the rejection, providing context or explanation for the decision.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The document number associated with the rejection, if applicable.
        /// </summary>
        public string DocNo { get; set; }
    }
}
