using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Bogus;
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

        [Theory(DisplayName = "Item quantity validation should fail for invalid values")]
        [InlineData(0, "Quantity must be at least 1.")]
        [InlineData(-1, "Quantity must be at least 1.")]
        [InlineData(21, "Maximum quantity per item is 20.")]
        [InlineData(100, "Maximum quantity per item is 20.")]
        public void InvalidQuantity_ShouldFail(int quantity, string expectedErrorMessage)
        {
            // Arrange
            var item = SaleTestData.GenerateValidSaleItem();
            item.Quantity = quantity;

            // Act
            var result = _validator.TestValidate(item);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Quantity)
                  .WithErrorMessage(expectedErrorMessage);
        }

        [Theory(DisplayName = "Item quantity validation should pass for valid values")]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(20)]
        public void ValidQuantity_ShouldPass(int quantity)
        {
            // Arrange
            var item = new Faker<SaleItem>()
                .CustomInstantiator(f => new SaleItem())
                .RuleFor(si => si.ProductId, f => Guid.NewGuid())
                .RuleFor(si => si.Quantity, quantity)
                .RuleFor(si => si.UnitPrice, f => f.Finance.Amount(1, 100, 2))
                .Generate();

            // Act
            var result = _validator.TestValidate(item);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Quantity);
        }
    }
}
