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

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.")
                .LessThanOrEqualTo(20).WithMessage("Maximum quantity is 20.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");


            RuleFor(saleItem => saleItem)
                .Must(item => item.Discount <= item.UnitPrice * item.Quantity)
                .WithMessage("Discount cannot be greater than the total value of the item.");

            RuleFor(saleItem => saleItem)
                .Must(item => item.Discount <= item.GrossTotal)
                .WithMessage("Discount cannot be greater than the total value of the item.");

            RuleFor(saleItem => saleItem)
                .Must(item => item.Quantity >= 4 || item.Discount == 0)
                .WithMessage("Discounts are only allowed for quantities of 4 or more.");
        }
    }
}
