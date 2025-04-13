using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

public class EntryCreditedEventPublisher :
    ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent>
{
    private readonly DaprClient _daprClient;

    public EntryCreditedEventPublisher(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task HandleAsync(
        IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        var payload = new EntryCreditedMessage
        {
            Id = domainEvent.AggregateIdentity.Value,
            Value = domainEvent.AggregateEvent.Value,
            Type = EventType.EntryCredited,
            Timestamp = domainEvent.Timestamp
        };

        await _daprClient.PublishEventAsync("pubsub", "entries.events", payload, cancellationToken);
    }
}
