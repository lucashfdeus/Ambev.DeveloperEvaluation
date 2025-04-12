using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand.SaleItemCommand> createSaleItemFaker =
            new Faker<CreateSaleCommand.SaleItemCommand>()
                .RuleFor(o => o.ProductId, f => Guid.NewGuid())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(10, 100));

        private static readonly Faker<CreateSaleCommand> createSaleFaker =
            new Faker<CreateSaleCommand>()
                .RuleFor(o => o.SaleNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(o => o.CustomerId, f => Guid.NewGuid())
                .RuleFor(o => o.CustomerName, f => f.Company.CompanyName())
                .RuleFor(o => o.BranchId, f => Guid.NewGuid())
                .RuleFor(o => o.BranchName, f => f.Company.CompanyName())
                .RuleFor(o => o.SaleDate, f => f.Date.Recent())
                .RuleFor(o => o.Items, f => createSaleItemFaker.Generate(f.Random.Int(1, 3)));

        public static CreateSaleCommand GenerateValidCommand()
        {
            return createSaleFaker.Generate();
        }
    }
}
