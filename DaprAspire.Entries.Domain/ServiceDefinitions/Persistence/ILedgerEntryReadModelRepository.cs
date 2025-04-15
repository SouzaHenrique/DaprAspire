using DaprAspire.Entries.Domain.ReadModels;

namespace DaprAspire.Entries.Domain.ServiceDefinitions.Persistence
{
    public interface ILedgerEntryReadModelRepository
    {
        /// <summary>
        /// Counts the number of ledger entries in the read model.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<long> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a paginated list of ledger entries from the read model.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IReadOnlyList<LedgerEntryReadModel>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    }
}
