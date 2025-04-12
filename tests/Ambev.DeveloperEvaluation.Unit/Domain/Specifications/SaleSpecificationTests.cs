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

        [Fact]
        public void Sale_With_Canceled_Items_Should_Exclude_Them_From_Total()
        {
            // Arrange
            var sale = SaleSpecificationTestData.GenerateSaleWithCanceledItems(2);

            while (sale.Items.Count < 3)
            {
                sale.Items.Add(new SaleItem { IsCancelled = false });
            }

            var activeItems = sale.Items.Where(i => !i.IsCancelled).ToList();
            var expectedTotal = activeItems.Sum(i => i.TotalItemAmount);

            // Act
            var actualTotal = sale.NetTotalAmount;

            // Assert
            Assert.Equal(expectedTotal, actualTotal);
            Assert.True(sale.Items.Count(i => i.IsCancelled) >= 2);
            Assert.True(activeItems.Count >= 1);
        }

        [Fact]
        public void NetTotalAmount_With_All_Items_Canceled_Should_Return_Zero()
        {
            var sale = SaleSpecificationTestData.GenerateSaleWithCanceledItems(5);
            sale.Items.ForEach(i => i.Cancel());

            Assert.Equal(0, sale.NetTotalAmount);
        }

        [Fact]
        public void NetTotalAmount_With_No_Canceled_Items_Should_Return_Full_Total()
        {
            var sale = SaleSpecificationTestData.GenerateSale(SaleStatus.Created);
            sale.Items.ForEach(i => i.IsCancelled = false);

            var expectedTotal = sale.Items.Sum(i => i.TotalItemAmount);
            Assert.Equal(expectedTotal, sale.NetTotalAmount);
        }
    }
}
