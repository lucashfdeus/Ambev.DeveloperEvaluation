using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class ProductInfoValidator : AbstractValidator<ProductInfo>
    {
        public ProductInfoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Product code is required.");
        }
    }
}
