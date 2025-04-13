using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product item within a sale.
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid ProductId { get;  set; } = Guid.Empty;

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get;  set; }

    public bool IsCancelled { get;  set; }

    public decimal GrossTotal => UnitPrice * Quantity;

    public decimal Discount => CalculateDiscount();

    public decimal TotalItemAmount => GrossTotal - Discount;

    public SaleItem() { }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
        };
    }

    private decimal CalculateDiscount()
    {
        if (Quantity >= 10) return GrossTotal * 0.20m;
        if (Quantity >= 4) return GrossTotal * 0.10m;
        return 0;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}
