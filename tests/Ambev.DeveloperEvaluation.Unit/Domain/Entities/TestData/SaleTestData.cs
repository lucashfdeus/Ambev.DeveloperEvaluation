using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
            .CustomInstantiator(f => new Sale())
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.SaleDate, f => f.Date.Recent(30))
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
            .RuleFor(s => s.CustomerName, f => f.Name.FullName())
            .RuleFor(s => s.BranchId, f => Guid.NewGuid())
            .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateValidSaleItems(f))
            .RuleFor(s => s.Status, SaleStatus.Created)
            .RuleFor(s => s.CreatedAt, f => f.Date.Past(1))
            .RuleFor(s => s.UpdatedAt, (f, s) => f.Date.Between(s.CreatedAt, DateTime.UtcNow));

        private static List<SaleItem> GenerateValidSaleItems(Faker f)
        {
            return new Faker<SaleItem>()
                .CustomInstantiator(fi => new SaleItem())
                .RuleFor(si => si.ProductId, fi => Guid.NewGuid())
                .RuleFor(si => si.ProductName, fi => f.Commerce.ProductName())
                .RuleFor(si => si.Quantity, fi => f.Random.Int(1, 3))
                .RuleFor(si => si.UnitPrice, fi => f.Finance.Amount(1, 100, 2))
                .RuleFor(si => si.IsCancelled, false)
                .Generate(f.Random.Int(1, 5));
        }

        public static Sale GenerateSaleWithItems(params SaleItem[] items)
        {
            var sale = SaleFaker.Generate();
            sale.Items.Clear();
            foreach (var item in items)
            {
                sale.AddItem(item);
            }
            return sale;
        }

        public static Sale GenerateSaleWithItemQuantities(params int[] quantities)
        {
            var items = new List<SaleItem>();
            foreach (var qty in quantities)
            {
                items.Add(new SaleItem
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = qty,
                    UnitPrice = 10m,
                    IsCancelled = false
                });
            }
            return GenerateSaleWithItems(items.ToArray());
        }

        public static Sale GenerateSaleWithDuplicateProducts(int productCount)
        {
            var productId = Guid.NewGuid();
            var items = new Faker<SaleItem>()
                .CustomInstantiator(fi => new SaleItem())
                .RuleFor(si => si.ProductId, productId)
                .RuleFor(si => si.Quantity, 1)
                .RuleFor(si => si.UnitPrice, 10m)
                .Generate(productCount);

            return GenerateSaleWithItems(items.ToArray());
        }

        public static SaleItem GenerateSaleItemForDiscountTest(int quantity)
        {
            return new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product",
                Quantity = quantity,
                UnitPrice = 100m,
                IsCancelled = false
            };
        }

        public static Sale GenerateSaleWithCancelledItems(int cancelledCount)
        {
            var sale = SaleFaker.Generate();
            for (int i = 0; i < cancelledCount && i < sale.Items.Count; i++)
            {
                sale.Items[i].IsCancelled = true;
            }
            return sale;
        }

        public static Sale GenerateValidSale()
        {
            return SaleFaker.Generate();
        }

        public static Sale GenerateEmptySale()
        {
            return new Sale();
        }

        public static Sale GenerateSaleWithNoItems()
        {
            return SaleFaker
                .RuleFor(s => s.Items, new List<SaleItem>())
                .Generate();
        }

        public static Sale GenerateSaleWithInvalidTotalAmount()
        {
            var sale = SaleFaker.Generate();
            sale.Items.ForEach(item => item.UnitPrice = -1 * Math.Abs(item.UnitPrice));
            return sale;
        }

        public static Sale GenerateSaleWithStatus(SaleStatus status)
        {
            return SaleFaker
                .RuleFor(s => s.Status, status)
                .Generate();
        }

        public static Sale GenerateSaleWithFutureDate()
        {
            return SaleFaker
                .RuleFor(s => s.SaleDate, f => f.Date.Future())
                .Generate();
        }

        public static Sale GenerateSaleWithCreatedAtAfterUpdatedAt()
        {
            return new Faker<Sale>()
                .CustomInstantiator(f => new Sale())
                .RuleFor(s => s.CreatedAt, f => f.Date.Recent(1))
                .RuleFor(s => s.UpdatedAt, (f, s) => f.Date.Past(1, s.CreatedAt))
                .Generate();
        }

        public static Sale GenerateSaleWithEmptyCustomerName()
        {
            return SaleFaker
                .RuleFor(s => s.CustomerName, string.Empty)
                .Generate();
        }

        public static Sale GenerateSaleWithEmptyBranchName()
        {
            return SaleFaker
                .RuleFor(s => s.BranchName, string.Empty)
                .Generate();
        }

        public static Sale GenerateSaleWithEmptySaleNumber()
        {
            return SaleFaker
                .RuleFor(s => s.SaleNumber, string.Empty)
                .Generate();
        }

        public static Sale GenerateSaleWithNullItems()
        {
            return new Faker<Sale>()
                .CustomInstantiator(f => new Sale())
                .RuleFor(s => s.Items, (List<SaleItem>)new())
                .Generate();
        }

        public static Sale GenerateSaleWithZeroQuantityItems()
        {
            var sale = SaleFaker.Generate();
            sale.Items.ForEach(item => item.Quantity = 0);
            return sale;
        }

        public static Sale GenerateSaleWithNegativeQuantityItems()
        {
            var sale = SaleFaker.Generate();
            sale.Items.ForEach(item => item.Quantity = -1 * Math.Abs(item.Quantity));
            return sale;
        }

        public static Sale GenerateSaleWithZeroPriceItems()
        {
            var sale = SaleFaker.Generate();
            sale.Items.ForEach(item => item.UnitPrice = 0);
            return sale;
        }

        public static Sale GenerateSaleWithNegativePriceItems()
        {
            var sale = SaleFaker.Generate();
            sale.Items.ForEach(item => item.UnitPrice = -1 * Math.Abs(item.UnitPrice));
            return sale;
        }

        public static string GenerateValidSaleNumber()
        {
            return new Faker().Random.AlphaNumeric(10);
        }

        public static string GenerateInvalidSaleNumber(int length)
        {
            return length < 10
                ? new Faker().Random.AlphaNumeric(length)
                : new Faker().Random.AlphaNumeric(length + 1);
        }

        public static string GenerateValidCustomerName()
        {
            return new Faker().Name.FullName();
        }

        public static string GenerateValidBranchName()
        {
            return new Faker().Company.CompanyName();
        }

        public static SaleItem GenerateValidSaleItem()
        {
            return new Faker<SaleItem>()
                .CustomInstantiator(f => new SaleItem())
                .RuleFor(si => si.ProductId, f => Guid.NewGuid())
                .RuleFor(si => si.ProductName, f => f.Commerce.ProductName())
                .RuleFor(si => si.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(si => si.UnitPrice, f => f.Finance.Amount(1, 100, 2))
                .Generate();
        }

        public static SaleItem GenerateInvalidSaleItem()
        {
            return new Faker<SaleItem>()
                .CustomInstantiator(f => new SaleItem())
                .RuleFor(si => si.ProductId, Guid.Empty)
                .RuleFor(si => si.ProductName, string.Empty)
                .RuleFor(si => si.Quantity, f => f.Random.Int(-10, 0))
                .RuleFor(si => si.UnitPrice, f => f.Finance.Amount(-100, 0, 2))
                .Generate();
        }
    }
}