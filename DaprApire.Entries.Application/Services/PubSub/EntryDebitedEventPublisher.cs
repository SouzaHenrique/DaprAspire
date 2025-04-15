using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

using Microsoft.Extensions.Logging;

public class EntryDebitedEventPublisher :
    ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<EntryDebitedEventPublisher> _logger;

    public EntryDebitedEventPublisher(DaprClient daprClient, ILogger<EntryDebitedEventPublisher> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task HandleAsync(
        IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing EntryDebitedEvent to Dapr pub/sub");

        var payload = new EntryDebitedMessage
        {
            Id = domainEvent.AggregateIdentity.Value,
            Value = domainEvent.AggregateEvent.Value,
            Type = EventType.EntryDebited,
            Timestamp = domainEvent.Timestamp,
            AggregateSequenceNumber = domainEvent.AggregateSequenceNumber,
        };

        await _daprClient.PublishEventAsync("pubsub", "entries.debited", payload, cancellationToken);
    }
}
