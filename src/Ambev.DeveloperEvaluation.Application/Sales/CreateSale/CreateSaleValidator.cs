using FluentValidation;
using static Ambev.DeveloperEvaluation.Application.Sales.CreateSale.CreateSaleCommand;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for user creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty();
        RuleFor(sale => sale.BranchId).NotEmpty(); 
        RuleFor(sale => sale.BranchName).NotEmpty();
        RuleFor(sale => sale.SaleDate).NotEmpty().LessThanOrEqualTo(DateTime.Now);

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemCommandValidator());
    }
}

public class SaleItemCommandValidator : AbstractValidator<SaleItemCommand>
{
    public SaleItemCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(item => item.ProductName)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
    }
}

