﻿using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Events.Sale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly CreateSaleHandler _handler;


        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CreateSaleHandler(_mapper, _saleRepository, _eventPublisher);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            // Given
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = command.SaleNumber,
            };

            var expectedResult = new CreateSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
            };

            _mapper.Map<Sale>(Arg.Is<CreateSaleCommand>(c => c.SaleNumber == command.SaleNumber)).Returns(sale);
            _mapper.Map<CreateSaleResult>(Arg.Is<Sale>(s => s.Id == sale.Id)).Returns(expectedResult);
            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
            _eventPublisher.PublishAsync(Arg.Any<SaleCreatedEvent>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Id.Should().Be(sale.Id);
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
            await _eventPublisher.Received(1).PublishAsync(Arg.Any<SaleCreatedEvent>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Given
            var invalidCommand = new CreateSaleCommand
            {
                SaleNumber = string.Empty,
                CustomerId = Guid.Empty,
                CustomerName = string.Empty,
                BranchId = Guid.Empty,
                BranchName = string.Empty,
                SaleDate = DateTime.MinValue
            };

            var validator = new CreateSaleCommandValidator();

            // When
            var act = () => _handler.Handle(invalidCommand, CancellationToken.None);

            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact(DisplayName = "Given valid sale creation request When handling Then publishes sale created event")]
        public async Task Handle_ValidRequest_PublishesSaleCreatedEvent()
        {
            // Given
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var sale = new Sale { Id = Guid.NewGuid() };
            _mapper.Map<Sale>(Arg.Any<CreateSaleCommand>()).Returns(sale);
            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

            // When
            await _handler.Handle(command, CancellationToken.None);

            // Then
            await _eventPublisher.Received(1).PublishAsync(Arg.Is<SaleCreatedEvent>(e => e.SaleId == sale.Id), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
        public async Task Handle_ValidRequest_MapsCommandToSale()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.GenerateValidCommand();
            var expectedSale = new Sale();

            expectedSale.Id = Guid.NewGuid();
            expectedSale.SaleNumber = command.SaleNumber;
            expectedSale.CustomerId = command.CustomerId;
            expectedSale.CustomerName = command.CustomerName;
            expectedSale.BranchId = command.BranchId;
            expectedSale.BranchName = command.BranchName;
            expectedSale.SaleDate = command.SaleDate;
            expectedSale.Status = SaleStatus.Created;
            expectedSale.CreatedAt = DateTime.UtcNow;
            expectedSale.UpdatedAt = DateTime.UtcNow;

            foreach (var item in command.Items)
            {
                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };
                expectedSale.AddItem(saleItem);
            }

            _mapper.Map<Sale>(command).Returns(expectedSale);
            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(expectedSale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
                c.SaleNumber == command.SaleNumber &&
                c.CustomerId == command.CustomerId &&
                c.CustomerName == command.CustomerName &&
                c.BranchId == command.BranchId &&
                c.BranchName == command.BranchName &&
                c.SaleDate == command.SaleDate &&
                c.Items.Count == command.Items.Count &&
                c.Items.All(expectedItem => command.Items.Any(actualItem =>
                    actualItem.ProductId == expectedItem.ProductId &&
                    actualItem.ProductName == expectedItem.ProductName &&
                    actualItem.Quantity == expectedItem.Quantity &&
                    actualItem.UnitPrice == expectedItem.UnitPrice))));
        }
    }
}