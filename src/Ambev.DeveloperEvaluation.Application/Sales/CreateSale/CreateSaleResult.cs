using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents the response returned after successfully creating a new sale.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CreateSaleResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public List<SaleItem> Items { get; set; } = new();
        public decimal GrossTotalAmount { get; set; }
        public decimal NetTotalAmount { get; set; }
        public SaleStatus Status { get; set; }
    }

    public class SaleItemResult
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }
        public bool IsCancelled { get; set; }
    }
}
