using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("SaleNumber must not be empty.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerExternalId must not be empty.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("CustomerName must not be empty.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("BranchExternalId must not be empty.");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("BranchName must not be empty.");

            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("SaleDate must not be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt)
                .WithMessage("UpdatedAt must be greater than or equal to CreatedAt.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid value.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items cannot be null.")
                .Must(items => items.Any()).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new SaleItemValidator());

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Total must be greater than zero.");
        }
    }
}
