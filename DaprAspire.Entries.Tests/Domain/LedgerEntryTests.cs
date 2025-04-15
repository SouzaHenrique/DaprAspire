using EventFlow.EventStores;
using EventFlow;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;
using EventFlow.Extensions;
using DaprApire.Entries.Application.Features.Commands;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using DaprAspire.Entries.Domain.Events;
using EventFlow.Aggregates;

namespace DaprAspire.Entries.Tests.Domain
{
    public class LedgerEntryTests
    {
        /// <summary>
        /// Tests the creation of a ledger entry and verifies that the EntryCreatedEvent is emitted.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateEntryCommand_ShouldEmit_EntryCreatedEvent()
        {
            // Arrange
            LedgerEntryId id = LedgerEntryId.New;
            ServiceProvider provider = EventFlowOptions.New()
                                           .AddDefaults(typeof(LedgerEntryAggregate).Assembly)
                                           .AddCommands(typeof(CreateEntryCommand))
                                           .AddCommandHandlers(typeof(CreateEntryCommandHandler))
                                           .ServiceCollection.BuildServiceProvider();

            ICommandBus? commandBus = provider.GetRequiredService<ICommandBus>();

            // Act
            await commandBus.PublishAsync(new CreateEntryCommand(id), CancellationToken.None);

            IEventStore? eventStore = provider.GetRequiredService<IEventStore>();

            IReadOnlyCollection<IDomainEvent<LedgerEntryAggregate, LedgerEntryId>> events = await eventStore
                .LoadEventsAsync<LedgerEntryAggregate, LedgerEntryId>(id, CancellationToken.None);

            // Assert
            events.Should().HaveCount(1);
            events.Should().ContainSingle(e => e.EventType == typeof(EntryCreatedEvent));
        }

        /// <summary>
        /// Tests the crediting of a ledger entry and verifies that the EntryCreditedEvent is emitted.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreditEntryCommand_ShouldEmit_EntryCreditedEvent()
        {
            // Arrange
            LedgerEntryId id = LedgerEntryId.New;
            ServiceProvider provider = EventFlowOptions.New()
                                           .AddDefaults(typeof(LedgerEntryAggregate).Assembly)
                                           .AddCommands(typeof(CreateEntryCommand), typeof(CreditEntryCommand))
                                           .AddCommandHandlers(typeof(CreateEntryCommandHandler), typeof(CreditEntryCommandHandler))
                                           .ServiceCollection.BuildServiceProvider();

            ICommandBus commandBus = provider.GetRequiredService<ICommandBus>();
            IEventStore eventStore = provider.GetRequiredService<IEventStore>();

            await commandBus.PublishAsync(new CreateEntryCommand(id), CancellationToken.None);

            // Act
            await commandBus.PublishAsync(new CreditEntryCommand(id, 100), CancellationToken.None);

            IReadOnlyCollection<IDomainEvent<LedgerEntryAggregate, LedgerEntryId>> events = await eventStore
                .LoadEventsAsync<LedgerEntryAggregate, LedgerEntryId>(id, CancellationToken.None);

            // Assert
            events.Should().HaveCount(2);
            events.Should().Contain(e => e.EventType == typeof(EntryCreditedEvent));
        }

        /// <summary>
        /// Tests the debiting of a ledger entry and verifies that the EntryDebitedEvent is emitted.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DebitEntryCommand_ShouldEmit_EntryDebitedEvent()
        {
            // Arrange
            LedgerEntryId id = LedgerEntryId.New;
            ServiceProvider provider = EventFlowOptions.New()
                                           .AddDefaults(typeof(LedgerEntryAggregate).Assembly)
                                           .AddCommands(typeof(CreateEntryCommand), typeof(DebitEntryCommand))
                                           .AddCommandHandlers(typeof(CreateEntryCommandHandler), typeof(DebitEntryCommandHandler))
                                           .ServiceCollection.BuildServiceProvider();

            ICommandBus commandBus = provider.GetRequiredService<ICommandBus>();
            IEventStore eventStore = provider.GetRequiredService<IEventStore>();

            await commandBus.PublishAsync(new CreateEntryCommand(id), CancellationToken.None);

            // Act
            await commandBus.PublishAsync(new DebitEntryCommand(id, 50), CancellationToken.None);

            var events = await eventStore.LoadEventsAsync<LedgerEntryAggregate, LedgerEntryId>(id, CancellationToken.None);

            // Assert
            events.Should().HaveCount(2);
            events.Should().Contain(e => e.EventType == typeof(EntryDebitedEvent));
        }


    }
}
