namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
    {
        public string SaleNumber { get; set; } = string.Empty;

        public Guid CustomerId { get; set; } = Guid.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public Guid BranchId { get; set; } = Guid.Empty;

        public string BranchName { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public List<CreateSaleItemRequest> Items { get; set; } = [];
    }

    public class CreateSaleItemRequest
    {
        public Guid ProductId { get; set; } = Guid.Empty;
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
