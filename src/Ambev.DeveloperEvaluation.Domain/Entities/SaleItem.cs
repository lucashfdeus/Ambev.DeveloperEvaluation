using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product item within a sale.
/// </summary>
public class SaleItem : BaseEntity
{
    public Guid ProductId { get;  private set; } = Guid.Empty;

    public string ProductName { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public bool IsCancelled { get; private set; }

    public decimal GrossTotal => UnitPrice * Quantity;

    public decimal Discount => CalculateDiscount();

    public decimal TotalItemAmount => GrossTotal - Discount;

    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;

        Validate();
    }


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
        if (Quantity >= 10)
            return UnitPrice * Quantity * 0.20m;

        if (Quantity >= 4)
            return UnitPrice * Quantity * 0.10m;

        return 0;
    }
}
