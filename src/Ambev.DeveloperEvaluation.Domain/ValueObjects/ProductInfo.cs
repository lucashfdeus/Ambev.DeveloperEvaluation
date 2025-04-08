using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public record ProductInfo
    {
        public string Name { get; }
        public string Code { get; }

        public ProductInfo(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new ProductInfoValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
