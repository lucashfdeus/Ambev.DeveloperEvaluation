using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sale
{
    public record SaleCreatedEvent(Guid SaleId, DateTime SaleDate, Guid CustomerId) : INotification;
}
