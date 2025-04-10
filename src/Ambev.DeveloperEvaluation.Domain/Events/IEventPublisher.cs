using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : INotification;
    }
}
