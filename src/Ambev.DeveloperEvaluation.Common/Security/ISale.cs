namespace Ambev.DeveloperEvaluation.Common.Security
{
    /// <summary>
    /// Defines the contract for representing a sale in the system.
    /// </summary>
    public interface ISale
    {
        string Id { get; }
        string SaleNumber { get; }
        DateTime Date { get; }
        string CustomerName { get; }
        string BranchName { get; }
        decimal TotalAmount { get; }
        string Status { get; }
    }
}
