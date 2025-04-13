using DaprAspire.Domain.Enums;

namespace DaprAspire.Domain.CrossCutting.Messaging.Events
{
    /// <summary>
    /// Represents a message indicating that an entry has been debited.
    /// </summary>
    public class EntryDebitedMessage : EntryEventMessage
    {
        public decimal Value { get; set; }
    }
}
