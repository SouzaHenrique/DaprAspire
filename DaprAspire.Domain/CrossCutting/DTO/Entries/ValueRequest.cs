﻿namespace DaprAspire.Domain.CrossCutting.DTO.Entries
{
    /// <summary>
    /// Represents a request to credit or debit a ledger entry with a specific value.
    /// </summary>
    public class ValueRequest
    {
        public decimal Value { get; set; }
    }
}
