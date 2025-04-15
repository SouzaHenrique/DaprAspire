using Dapr.Client;

using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Domain.Enums;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Events;

using EventFlow.Aggregates;
using EventFlow.Subscribers;

using Microsoft.Extensions.Logging;

public class EntryCreditedEventPublisher :
    ISubscribeSynchronousTo<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<EntryCreditedEventPublisher> _logger;

    public EntryCreditedEventPublisher(DaprClient daprClient, ILogger<EntryCreditedEventPublisher> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task HandleAsync(
        IDomainEvent<LedgerEntryAggregate, LedgerEntryId, EntryCreditedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing EntryCreditedEvent to Dapr pub/sub");

        var payload = new EntryCreditedMessage
        {
            Id = domainEvent.AggregateIdentity.Value,
            Value = domainEvent.AggregateEvent.Value,
            Type = EventType.EntryCredited,
            Timestamp = domainEvent.Timestamp,
            AggregateSequenceNumber = domainEvent.AggregateSequenceNumber,
        };

        await _daprClient.PublishEventAsync("pubsub", "entries.credited", payload, cancellationToken);
    }
}
