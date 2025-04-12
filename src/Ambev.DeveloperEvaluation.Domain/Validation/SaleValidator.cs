using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleValidator : AbstractValidator<Sale>
    {
        public SaleValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("SaleNumber must not be empty.")
                .MaximumLength(50).WithMessage("SaleNumber cannot exceed 50 characters.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID must not be empty.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name must not be empty.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(x => x.BranchId)
                .NotEmpty().WithMessage("Branch ID must not be empty.");

            RuleFor(x => x.BranchName)
                .NotEmpty().WithMessage("Branch name must not be empty.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date must not be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(x => x.CreatedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Creation date cannot be in the future.");

            RuleFor(x => x.UpdatedAt)
                .GreaterThanOrEqualTo(x => x.CreatedAt)
                .WithMessage("Update date must be greater than or equal to creation date.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid value.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items list cannot be null.")
                .Must(items => items.Any()).WithMessage("At least one item is required.")
                .Must(items => items.GroupBy(i => i.ProductId)
                .All(g => g.Sum(i => i.Quantity) <= 20))
                .WithMessage("Maximum quantity of 20 identical items exceeded.");

            RuleForEach(x => x.Items)
                .SetValidator(new SaleItemValidator());

            RuleFor(x => x.GrossTotalAmount)
                .GreaterThan(0).WithMessage("Gross total must be greater than zero.");

            RuleFor(x => x.NetTotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Net total cannot be negative.");
        }
    }
}