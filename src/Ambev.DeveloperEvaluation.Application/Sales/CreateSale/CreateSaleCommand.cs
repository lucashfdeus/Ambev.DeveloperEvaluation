using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for creating a saler. 
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="CreateSaleResult"/>.
    /// 
    /// The data provided in this command is validated using the 
    /// <see cref="CreateUserCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and follow the required rules.
    /// </remarks>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string SaleNumber { get; set; } = string.Empty;

        public Guid CustomerId { get; set; } = Guid.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; } = Guid.Empty;

        public string BranchName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public List<SaleItemCommand> Items { get; set; } = new();

        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }

        public class SaleItemCommand
        {
            public Guid ProductId { get; set; } = Guid.Empty;
            public string ProductName { get; set; } = string.Empty;

            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }

            public ValidationResultDetail Validate()
            {
                var validator = new SaleItemCommandValidator();
                var result = validator.Validate(this);
                return new ValidationResultDetail
                {
                    IsValid = result.IsValid,
                    Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
                };
            }
        }
    }
}