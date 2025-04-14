using DaprAspire.Consolidation.Domain.Projections;
using DaprAspire.Domain.CrossCutting.Messaging.Events;

namespace DaprAspire.Consolidation.Application.Services.ProjectionService
{
    /// <summary>
    /// Interface for the entry projection service.
    /// </summary>
    public interface IEntryProjectionService
    {
        /// <summary>
        /// Apply the entry created message to the projection.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task ApplyAsync(EntryCreatedMessage message);

        /// <summary>
        /// Apply the entry credited message to the projection.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task ApplyAsync(EntryCreditedMessage message);

        /// <summary>
        /// Apply the entry debited message to the projection.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task ApplyAsync(EntryDebitedMessage message);

        /// <summary>
        /// Get the latest daily balance projection for a given ledger ID.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<DailyBalanceProjection?> GetLatestDailyBalanceAsync(string ledgerId, CancellationToken cancellationToken = default);
    }
}
