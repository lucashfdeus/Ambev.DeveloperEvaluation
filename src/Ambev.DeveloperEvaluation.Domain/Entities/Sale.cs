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

        public Guid CustomerId { get; set; } = Guid.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; } = Guid.Empty;

        public string BranchName { get; set; } = string.Empty;

        public List<SaleItem> Items { get; private set; } = new();

        private decimal _grossTotalAmount;
        public decimal GrossTotalAmount
        {
            get => _grossTotalAmount;
            private set => _grossTotalAmount = value;
        }

        private decimal _netTotalAmount;
        public decimal NetTotalAmount
        {
            get => _netTotalAmount;
            private set => _netTotalAmount = value;
        }


        public SaleStatus Status { get; set; } = SaleStatus.Created;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Sale() { }

        public Sale(List<SaleItem> items)
        {
            Items = items;
            RecalculateTotals();
        }

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

        public void AddItem(SaleItem item)
        {
            var totalQuantity = Items
                .Where(i => i.ProductId == item.ProductId && !i.IsCancelled)
                .Sum(i => i.Quantity) + item.Quantity;

            Items.Add(item);
            RecalculateTotals();
            UpdatedAt = DateTime.UtcNow;
        }

        public void CancelItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null || item.IsCancelled) return;

            item.Cancel();
            RecalculateTotals();
            UpdatedAt = DateTime.UtcNow;
        }

        private void RecalculateTotals()
        {
            GrossTotalAmount = Items.Sum(i => i.GrossTotal);
            NetTotalAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.NetTotal);
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