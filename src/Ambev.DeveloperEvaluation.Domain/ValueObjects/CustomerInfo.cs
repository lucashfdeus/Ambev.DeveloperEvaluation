using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public record CustomerInfo
    {
        public string Name { get; }
        public string Document { get; }

        public CustomerInfo(string name, string document)
        {
            Name = name;
            Document = document;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new CustomerInfoValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
