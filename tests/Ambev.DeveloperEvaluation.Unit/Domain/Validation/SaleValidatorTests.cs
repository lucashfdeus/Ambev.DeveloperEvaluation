using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleValidatorTests
    {
        private readonly SaleValidator _validator;

        public SaleValidatorTests()
        {
            _validator = new SaleValidator();
        }

        [Fact(DisplayName = "Sale with canceled items should exclude from NetTotal")]
        public void CanceledItems_ShouldExcludeFromNetTotal()
        {
            // Arrange
            var sale = SaleTestData.GenerateSaleWithCancelledItems(1);

            // Act & Assert
            _validator.TestValidate(sale)
                .ShouldNotHaveValidationErrorFor(x => x.NetTotalAmount);
        }

        [Theory(DisplayName = "Sale with invalid number should fail")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void InvalidSaleNumber_ShouldFail(string saleNumber)
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleNumber = saleNumber;

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.SaleNumber)
                  .WithErrorMessage("SaleNumber must not be empty.");
        }

        [Fact(DisplayName = "Sale with future date should fail")]
        public void FutureSaleDate_ShouldFail()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.SaleDate = DateTime.UtcNow.AddDays(1);

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.SaleDate)
                  .WithErrorMessage("Sale date cannot be in the future.");
        }

        [Fact(DisplayName = "Sale with no items should fail")]
        public void EmptyItems_ShouldFail()
        {
            var sale = SaleTestData.GenerateSaleWithNoItems();

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.Items)
                  .WithErrorMessage("At least one item is required.");
        }

        [Fact(DisplayName = "Sale with future CreatedAt should fail validation")]
        public void FutureCreatedAt_ShouldFail()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.CreatedAt = DateTime.UtcNow.AddHours(1);

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.CreatedAt)
                  .WithErrorMessage("Creation date cannot be in the future.");
        }

        [Fact(DisplayName = "Sale with UpdatedAt before CreatedAt should fail validation")]
        public void UpdatedAtBeforeCreatedAt_ShouldFail()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.UpdatedAt = sale.CreatedAt.AddMinutes(-1);

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.UpdatedAt)
                  .WithErrorMessage("Update date must be greater than or equal to creation date.");
        }

        [Fact(DisplayName = "Sale with invalid status should fail")]
        public void InvalidStatus_ShouldFail()
        {
            var sale = SaleTestData.GenerateValidSale();
            sale.Status = (SaleStatus)999; // Invalid value

            var result = _validator.TestValidate(sale);

            result.ShouldHaveValidationErrorFor(x => x.Status)
                  .WithErrorMessage("Status must be a valid value.");
        }

        [Fact(DisplayName = "Sale with zero GrossTotal should fail validation")]
        public void ZeroGrossTotal_ShouldFail()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            sale.Items.ForEach(i => i.UnitPrice = 0);

            // Act
            var result = _validator.TestValidate(sale);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.GrossTotalAmount)
                  .WithErrorMessage("Gross total must be greater than zero.");
        }
    }
}
