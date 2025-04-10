using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly IMapper _mapper;
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        public CreateSaleHandler(IMapper mapper, ISaleRepository saleRepository, IEventPublisher eventPublisher)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = _mapper.Map<Sale>(command);

            sale.Status = SaleStatus.Created;

            sale.CreatedAt = DateTime.UtcNow;

            sale.UpdatedAt = sale.CreatedAt;

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            if(createdSale.Status == SaleStatus.Created)
            {
                await _eventPublisher.PublishAsync(new SaleCreatedEvent(createdSale.Id, createdSale.SaleDate, createdSale.CustomerId), cancellationToken);
            }

            var result = _mapper.Map<CreateSaleResult>(createdSale);

            return result;
        }
    }
}
