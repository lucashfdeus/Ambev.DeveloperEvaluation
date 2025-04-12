using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        #region Finalize Sale Tests
        [Fact]
        public void FinalizeSale_WhenSaleIsCreated_ShouldUpdateToFinalized()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Created);
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.FinalizeSale();

            // Assert
            Assert.Equal(SaleStatus.Finalized, sale.Status);
            Assert.True(sale.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void FinalizeSale_WhenSaleIsAlreadyFinalized_ShouldThrowDomainException()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Finalized);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.FinalizeSale());
            Assert.Equal("Only a created sale can be finalized.", ex.Message);
        }

        [Fact]
        public void FinalizeSale_WhenSaleIsCanceled_ShouldThrowDomainException()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Canceled);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.FinalizeSale());
            Assert.Equal("Only a created sale can be finalized.", ex.Message);
        }
        #endregion

        #region Cancel Sale Tests
        [Fact]
        public void Cancel_WhenSaleIsCreated_ShouldUpdateToCanceled()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Created);
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.Cancel();

            // Assert
            Assert.Equal(SaleStatus.Canceled, sale.Status);
            Assert.True(sale.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void Cancel_WhenSaleIsAlreadyCanceled_ShouldDoNothing()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Canceled);
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.Cancel();

            // Assert
            Assert.Equal(SaleStatus.Canceled, sale.Status);
            Assert.Equal(originalUpdatedAt, sale.UpdatedAt);
        }

        [Fact]
        public void Cancel_WhenSaleIsFinalized_ShouldUpdateToCanceled()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Finalized);
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.Cancel();

            // Assert
            Assert.Equal(SaleStatus.Canceled, sale.Status);
            Assert.True(sale.UpdatedAt > originalUpdatedAt);
        }
        #endregion

        #region Total Amount Tests Sale

        [Fact]
        public void NetTotalAmount_Should_Include_Discounts()
        {
            var sale = new Sale
            {
                Items = new List<SaleItem>
            {
                new() { Quantity = 5, UnitPrice = 100m },  // 10% discount
                new() { Quantity = 2, UnitPrice = 50m }    // No discount
            }
            };

            Assert.Equal(550m, sale.NetTotalAmount);
        }

        [Fact]
        public void GrossTotalAmount_Should_Ignore_Discounts()
        {
            var sale = new Sale
            {
                Items = new List<SaleItem>
            {
                new() { Quantity = 5, UnitPrice = 100m },
                new() { Quantity = 2, UnitPrice = 50m }   
            }
            };

            Assert.Equal(600m, sale.GrossTotalAmount);
        }

        [Fact]
        public void EmptySale_Should_Return_Zero_Totals()
        {
            var sale = new Sale { Items = new List<SaleItem>() };

            Assert.Equal(0m, sale.NetTotalAmount);
            Assert.Equal(0m, sale.GrossTotalAmount);
        }
        #endregion
    }
}
