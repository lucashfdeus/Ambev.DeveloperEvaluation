using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation
{
    public class SaleItemValidatorTests
    {
        private readonly SaleItemValidator _validator;

        public SaleItemValidatorTests()
        {
            _validator = new SaleItemValidator();
        }

        [Fact(DisplayName = "Valid sale item should pass all validations")]
        public void ValidItem_ShouldPassAllValidations()
        {
            var item = SaleTestData.GenerateValidSaleItem();

            var result = _validator.TestValidate(item);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory(DisplayName = "Item with invalid ProductId should fail")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void InvalidProductId_ShouldFail(string productId)
        {
            // Arrange
            var item = new SaleItem
            {
                ProductName = "Test Product",
                Quantity = 1,
                UnitPrice = 10.0m,
                ProductId = string.IsNullOrWhiteSpace(productId) ? Guid.Empty : Guid.NewGuid()
            };

            // Act
            var result = _validator.TestValidate(item);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProductId)
                  .WithErrorMessage("ProductId must not be empty.");
        }

        [Theory(DisplayName = "Item with invalid quantity should fail")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(21)]
        public void InvalidQuantity_ShouldFail(int quantity)
        {
            var item = SaleTestData.GenerateValidSaleItem();
            item.Quantity = quantity;

            var result = _validator.TestValidate(item);

            if (quantity <= 0)
            {
                result.ShouldHaveValidationErrorFor(x => x.Quantity)
                      .WithErrorMessage("Quantity must be at least 1.");
            }
            else
            {
                result.ShouldHaveValidationErrorFor(x => x.Quantity)
                      .WithErrorMessage("Maximum quantity is 20.");
            }
        }
    }
}
