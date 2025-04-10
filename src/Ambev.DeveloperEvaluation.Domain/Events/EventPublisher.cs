using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IMediator _mediator;

        public EventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
          where TEvent : INotification
        {
            return _mediator.Publish(@event, cancellationToken);
        }
    }
}
