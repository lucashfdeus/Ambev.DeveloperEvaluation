using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a product item within a sale.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        public Guid SaleId { get; set; }  //EF Core
        public Sale Sale { get; set; }    // Propriedade de navegação

        public Guid ProductId { get; set; } = Guid.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                UpdateTotals();
            }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                _unitPrice = value;
                UpdateTotals();
            }
        }

        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }     

        public SaleItem() { }

        private void UpdateTotals()
        {
            GrossTotal = UnitPrice * Quantity;
            Discount = CalculateDiscount();
            NetTotal = GrossTotal - Discount;
        }

        private decimal CalculateDiscount()
        {
            if (Quantity >= 10) return GrossTotal * 0.20m;
            if (Quantity >= 4) return GrossTotal * 0.10m;
            return 0;
        }

        public void Cancel() => IsCancelled = true;

        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}