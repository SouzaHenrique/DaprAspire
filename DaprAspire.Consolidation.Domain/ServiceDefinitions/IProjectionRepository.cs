using DaprAspire.Consolidation.Domain.Projections;

namespace DaprAspire.Consolidation.Domain.ServiceDefinitions
{
    public interface IProjectionRepository<T> where T : Projection
    {
        /// <summary>
        /// Get a projection by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a projection by its LedgerId.
        /// </summary>
        /// <param name="ledgerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T?> GetByLedgerIdAsync(string ledgerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upsert a projection. If the projection does not exist, it will be created. If it does exist, it will be updated.
        /// </summary>
        /// <param name="projection"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task UpsertAsync(T projection, CancellationToken cancellationToken = default);
    }
}
