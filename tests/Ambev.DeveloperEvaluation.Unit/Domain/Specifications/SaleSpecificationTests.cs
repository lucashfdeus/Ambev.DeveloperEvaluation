using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Specifications.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Specifications
{
    public class SaleSpecificationTests
    {
        [Theory]
        [InlineData(SaleStatus.Created)]
        [InlineData(SaleStatus.Finalized)]
        [InlineData(SaleStatus.Canceled)]
        public void Sale_Should_Have_Correct_Status_When_Generated(SaleStatus expectedStatus)
        {
            // Arrange
            var sale = SaleSpecificationTestData.GenerateSale(expectedStatus);

            // Act
            var actualStatus = sale.Status;

            // Assert
            Assert.Equal(expectedStatus, actualStatus);
            Assert.NotNull(sale.SaleNumber);
            Assert.NotEmpty(sale.SaleNumber);
            Assert.True(sale.SaleDate <= DateTime.UtcNow);
        }

        [Fact(DisplayName = "Sale With Canceled Items Should Exclude Them From NetTotal")]
        public void Sale_With_Canceled_Items_Should_Exclude_Them_From_Total()
        {
            // Arrange
            var sale = new Sale();

            var canceledItem1 = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 50m
            };
            canceledItem1.Cancel();
            sale.AddItem(canceledItem1);

            var canceledItem2 = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 3,
                UnitPrice = 30m
            };
            canceledItem2.Cancel();
            sale.AddItem(canceledItem2);

            var activeItem1 = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 20.15m
            };
            sale.AddItem(activeItem1);

            var activeItem2 = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 4,
                UnitPrice = 10m
            };
            sale.AddItem(activeItem2);

            var expectedTotal = sale.Items.Where(i => !i.IsCancelled).Sum(i => i.NetTotal);

            // Act & Assert
            Assert.Equal(expectedTotal, sale.NetTotalAmount);
            Assert.Equal(2, sale.Items.Count(i => i.IsCancelled));
            Assert.True(sale.Items.Count(i => !i.IsCancelled) >= 1);
        }

        [Fact]
        public void NetTotalAmount_With_All_Items_Canceled_Should_Return_Zero()
        {
            var sale = SaleSpecificationTestData.GenerateSaleWithCanceledItems(5);
            sale.Items.ForEach(i => i.Cancel());

            Assert.Equal(0, sale.NetTotalAmount);
        }

        [Fact(DisplayName = "NetTotalAmount Should Exclude Canceled Items")]
        public void NetTotalAmount_Should_Exclude_Canceled_Items()
        {
            var sale = new Sale();

            // Item ativo
            sale.AddItem(new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = 20.15m,
                IsCancelled = false
            });

            var canceledItem = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 50m
            };

            canceledItem.Cancel();
            sale.AddItem(canceledItem);

            // Act & Assert
            Assert.Equal(20.15m, sale.NetTotalAmount);
        }
    }
}
