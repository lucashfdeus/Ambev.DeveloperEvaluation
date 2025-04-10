using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction in the system.
    /// Contains key sale information and business rules validation.
    /// </summary>
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; }

        public Guid CustomerId { get;  set; } = Guid.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; } = Guid.Empty;

        public string BranchName { get; set; } = string.Empty;

        public List<SaleItem> Items { get; set; } = [];

        public decimal TotalAmount => Items.Sum(i => i.TotalItemAmount);

        public SaleStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Sale() { }

        public void Cancel()
        {
            if (Status == SaleStatus.Canceled)
                return;

            Status = SaleStatus.Canceled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void FinalizeSale()
        {
            if (Status != SaleStatus.Created)
                throw new DomainException("Only a created sale can be finalized.");

            Status = SaleStatus.Finalized;
            UpdatedAt = DateTime.UtcNow;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}