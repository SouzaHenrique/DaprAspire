using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

namespace DaprApire.Entries.Application.Services.PubSub
{
    public class EntryCreatedEventPublisher(DaprClient daprClient) :
        ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent>
    {
        private readonly DaprClient _daprClient = daprClient;

        public async Task HandleAsync(IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            var payload = new EntryCreatedMessage
            {
                Id = domainEvent.AggregateIdentity.Value,
                Timestamp = domainEvent.Timestamp,
                Type = EventType.EntryCreated
            };

            await _daprClient.PublishEventAsync("pubsub", "entries.events", payload, cancellationToken);
        }
    }
}
