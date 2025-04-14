using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Command for retrieving a sale by their ID
    /// </summary>
    public record GetSaleCommand : IRequest<GetSaleResult>
    {
        public Guid Id { get; }

        public GetSaleCommand(Guid id)
        {
            Id = id;
        }
    }
}
