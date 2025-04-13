using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData
{
    public static class SaleSpecificationTestData
    {
        private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale())
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => GenerateSaleItems(f))
        .RuleFor(s => s.Status, f => f.PickRandom<SaleStatus>())
        .RuleFor(s => s.CreatedAt, f => f.Date.Past(1))
        .RuleFor(s => s.UpdatedAt, (f, s) => f.Date.Between(s.CreatedAt, DateTime.UtcNow));

        private static readonly Faker<Sale> ValidSaleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale())
        .RuleFor(s => s.Items, f => new Faker<SaleItem>()
        .CustomInstantiator(fi => new SaleItem())
        .RuleFor(si => si.Quantity, 1)
        .RuleFor(si => si.UnitPrice, f.Finance.Amount(0.01m, 100, 2))
        .Generate(f.Random.Int(1, 3)));

        public static Sale GenerateGuaranteedValidSale()
        {
            return ValidSaleFaker.Generate();
        }

        private static List<SaleItem> GenerateSaleItems(Faker f)
        {
            return new Faker<SaleItem>()
                .CustomInstantiator(fi => new SaleItem())
                .RuleFor(si => si.ProductId, fi => Guid.NewGuid())
                .RuleFor(si => si.ProductName, fi => f.Commerce.ProductName())
                .RuleFor(si => si.Quantity, fi => f.Random.Int(1, 10))
                .RuleFor(si => si.UnitPrice, fi => Math.Round(f.Random.Decimal(1, 100), 2))
                .RuleFor(si => si.IsCancelled, false)
                .Generate(f.Random.Int(1, 5));
        }

        public static Sale GenerateSale(SaleStatus status)
        {
            var sale = saleFaker.Generate();
            sale.Status = status;
            return sale;
        }

        public static Sale GenerateSaleWithCanceledItems(int canceledItemCount)
        {
            var sale = saleFaker.Generate();

            while (sale.Items.Count < canceledItemCount)
            {
                sale.Items.Add(GenerateValidSaleItem());
            }

            sale.Items.Take(canceledItemCount).ToList().ForEach(item => item.Cancel());
            return sale;
        }

        private static SaleItem GenerateValidSaleItem()
        {
            return new Faker<SaleItem>()
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(1, 100))
                .Generate();
        }
    }
}
