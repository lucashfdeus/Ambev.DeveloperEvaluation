using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class BranchInfoValidator : AbstractValidator<BranchInfo>
    {
        public BranchInfoValidator()
        {
            RuleFor(b => b.Code)
           .NotEmpty().WithMessage("Branch code is required.");

            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");
        }
    }
}
