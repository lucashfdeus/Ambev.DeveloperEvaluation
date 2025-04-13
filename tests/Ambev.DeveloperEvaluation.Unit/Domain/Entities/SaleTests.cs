using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        #region AddItem Tests
        [Fact]
        public void AddItem_WhenSaleIsCreated_ShouldAddItem()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Created);
            var item = new SaleItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m };
            var originalItemCount = sale.Items.Count;
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.AddItem(item);

            // Assert
            Assert.Equal(originalItemCount + 1, sale.Items.Count);
            Assert.True(sale.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void AddItem_WhenSaleIsNotCreated_ShouldThrowDomainException()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Finalized);
            var item = new SaleItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m };

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(item));
            Assert.Equal("Can only add items to created sales.", ex.Message);
        }

        [Fact]
        public void AddItem_WhenExceeds20IdenticalItems_ShouldThrowDomainException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var sale = SaleTestData.GenerateSaleWithItems(
                new SaleItem { ProductId = productId, Quantity = 18, UnitPrice = 10m });
            var newItem = new SaleItem { ProductId = productId, Quantity = 3, UnitPrice = 10m };

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => sale.AddItem(newItem));
            Assert.Equal("Maximum of 20 identical items per sale.", ex.Message);
        }
        #endregion

        #region CancelItem Tests
        [Fact]
        public void CancelItem_WhenItemExists_ShouldMarkAsCancelled()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var sale = SaleTestData.GenerateSaleWithItems(
                new SaleItem { ProductId = productId, Quantity = 2, UnitPrice = 50m });
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.CancelItem(productId);

            // Assert
            var item = sale.Items.First(i => i.ProductId == productId);
            Assert.True(item.IsCancelled);
            Assert.True(sale.UpdatedAt > originalUpdatedAt);
        }

        [Fact]
        public void CancelItem_WhenItemAlreadyCancelled_ShouldDoNothing()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var sale = SaleTestData.GenerateSaleWithItems(
                new SaleItem { ProductId = productId, Quantity = 1, UnitPrice = 30m, IsCancelled = true });
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.CancelItem(productId);

            // Assert
            Assert.Equal(originalUpdatedAt, sale.UpdatedAt);
        }

        [Fact]
        public void CancelItem_WhenItemDoesNotExist_ShouldDoNothing()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithStatus(SaleStatus.Created);
            var originalUpdatedAt = sale.UpdatedAt;

            // Act
            sale.CancelItem(Guid.NewGuid());

            // Assert
            Assert.Equal(originalUpdatedAt, sale.UpdatedAt);
        }
        #endregion

        #region NetTotalAmount Edge Cases
        [Fact]
        public void NetTotalAmount_Should_Exclude_Cancelled_Items()
        {
            var sale = new Sale
            {
                Items = new List<SaleItem>
                {
                    new() { Quantity = 5, UnitPrice = 100m }, // 10% discount = 450
                    new() { Quantity = 2, UnitPrice = 50m, IsCancelled = true } // Cancelled = 0
                }
            };

            Assert.Equal(450m, sale.NetTotalAmount);
        }

        [Fact]
        public void NetTotalAmount_With_Max_Discount_Should_Calculate_Correctly()
        {
            var sale = new Sale
            {
                Items = new List<SaleItem>
                {
                    new() { Quantity = 10, UnitPrice = 100m } // 20% discount = 800
                }
            };

            Assert.Equal(800m, sale.NetTotalAmount);
        }
        #endregion

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
