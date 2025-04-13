using DaprAspire.Domain.Enums;

namespace DaprAspire.Domain.CrossCutting.Messaging.Events
{
    /// <summary>
    /// Base class for entry event messages.
    /// </summary>
    public class EntryEventMessage
    {
        public string Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public EventType Type { get; set; }
    }
}
