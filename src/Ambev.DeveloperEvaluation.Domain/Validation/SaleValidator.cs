using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId must not be empty.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchId must not be empty.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items cannot be null.")
                .Must(items => items.Any()).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new SaleItemValidator());

            RuleFor(x => x.Total)
                .GreaterThan(0).WithMessage("Total must be greater than zero.");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");

            RuleFor(x => x)
                .Must(sale => sale.Total >= sale.Discount)
                .WithMessage("Total must be greater than or equal to Discount.");

            RuleFor(x => x.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");
        }
    }
}
