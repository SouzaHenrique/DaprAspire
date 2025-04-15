using System.ComponentModel;

namespace DaprAspire.Domain.Enums
{
    public enum EventType
    {
        [Description("Entry Created")]
        EntryCreated,

        [Description("Entry Credited")]
        EntryCredited,

        [Description("Entry Debited")]
        EntryDebited
    }
}
