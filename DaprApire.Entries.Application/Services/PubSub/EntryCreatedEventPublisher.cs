using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using DnsClient.Internal;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

using Microsoft.Extensions.Logging;

namespace DaprApire.Entries.Application.Services.PubSub
{
    public class EntryCreatedEventPublisher(DaprClient daprClient, ILogger<EntryCreatedEventPublisher> logger) :
        ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent>
    {
        private readonly DaprClient _daprClient = daprClient;
        private readonly ILogger<EntryCreatedEventPublisher> _logger = logger;

        public async Task HandleAsync(IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreatedEvent> domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing EntryCreatedEvent to Dapr pub/sub");

            var payload = new EntryCreatedMessage
            {
                Id = domainEvent.AggregateIdentity.Value,
                Timestamp = domainEvent.Timestamp,
                Type = EventType.EntryCreated,
                AggregateSequenceNumber = domainEvent.AggregateSequenceNumber,
            };

            await _daprClient.PublishEventAsync("pubsub", "entries.created", payload, cancellationToken);
        }
    }
}
