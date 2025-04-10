namespace Ambev.DeveloperEvaluation.Domain.Enums
{
    /// <summary>
    /// Represents the current status of a sale.
    /// </summary>
    public enum SaleStatus
    {
        /// <summary>
        /// The sale has been created and can be modified.
        /// </summary>
        Created = 1,

        /// <summary>
        /// The sale has been finalized and can no longer be changed.
        /// </summary>
        Finalized = 2,

        /// <summary>
        /// The sale has been canceled and is no longer valid.
        /// </summary>
        Canceled = 3
    }
}
