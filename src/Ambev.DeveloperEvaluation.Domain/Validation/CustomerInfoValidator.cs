using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class CustomerInfoValidator : AbstractValidator<CustomerInfo>
    {
        public CustomerInfoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Customer name is required.");

            RuleFor(c => c.Document)
                .NotEmpty().WithMessage("Customer document is required.");
        }
    }
}
