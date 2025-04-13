using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

public class EntryDebitedEventPublisher :
    ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent>
{
    private readonly DaprClient _daprClient;

    public EntryDebitedEventPublisher(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task HandleAsync(
        IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryDebitedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        var payload = new EntryDebitedMessage
        {
            Id = domainEvent.AggregateIdentity.Value,
            Value = domainEvent.AggregateEvent.Value,
            Type = EventType.EntryDebited,
            Timestamp = domainEvent.Timestamp
        };

        await _daprClient.PublishEventAsync("pubsub", "entries.events", payload, cancellationToken);
    }
}
