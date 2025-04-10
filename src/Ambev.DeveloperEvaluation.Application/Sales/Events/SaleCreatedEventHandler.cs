using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
    {
        public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[Evento] Venda criada: {notification.SaleId}, Cliente: {notification.CustomerId}");

            return Task.CompletedTask;
        }
    }
}
