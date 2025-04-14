using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedEvent>
    {
        public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Event] SaleCreated: {notification.SaleId}, Client: {notification.CustomerId}");
            Console.ForegroundColor = previousColor;
            return Task.CompletedTask;
        }
    }
}
