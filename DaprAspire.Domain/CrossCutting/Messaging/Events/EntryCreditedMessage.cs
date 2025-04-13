using DaprAspire.Domain.Enums;

namespace DaprAspire.Domain.CrossCutting.Messaging.Events
{
    /// <summary>
    /// Represents a message for an entry credited event.
    /// </summary>
    public class EntryCreditedMessage : EntryEventMessage
    {
        public decimal Value { get; set; }
    }
}
