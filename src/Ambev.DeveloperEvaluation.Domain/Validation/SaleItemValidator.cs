using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        public SaleItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId must not be empty.");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.")
                .LessThanOrEqualTo(20).WithMessage("Maximum quantity per item is 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
                .Must((item, discount) =>
                    item.Quantity < 4 ? discount == 0 :
                    item.Quantity < 10 ? discount <= item.GrossTotal * 0.10m :
                    discount <= item.GrossTotal * 0.20m)
                .WithMessage("Discount exceeds allowed limit for this quantity (0% <4 items, 10% 4-9 items, 20% 10+ items).");

            RuleFor(x => x.GrossTotal)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");
        }
    }
}