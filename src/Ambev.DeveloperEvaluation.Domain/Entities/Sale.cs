using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale transaction in the system.
/// Contains key sale information and business rules validation.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number.
    /// Used for tracking and identification.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date of the sale.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets the unique identifier of the customer.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the branch where the sale occurred.
    /// </summary>
    public string BranchId { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of items included in the sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Gets the total amount of the sale including discounts.
    /// </summary>
    public decimal Total { get; set; }

    /// <summary>
    /// Gets the discount applied to the sale.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets the status of the sale.
    /// Indicates whether the sale is finalized or canceled.
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// Gets the creation timestamp of the sale.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the last update timestamp of the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
        Status = SaleStatus.Pending;
    }

    /// <summary>
    /// Cancels the sale.
    /// Changes the status to Canceled.
    /// </summary>
    public void Cancel()
    {
        Status = SaleStatus.Canceled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Finalizes the sale.
    /// Changes the status to Finalized.
    /// </summary>
    public void FinalizeSale()
    {
        Status = SaleStatus.Finalized;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates the sale using the SaleValidator rules.
    /// </summary>
    /// <returns>A ValidationResultDetail containing validation results.</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }
}
