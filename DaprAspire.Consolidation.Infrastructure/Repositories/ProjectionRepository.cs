using DaprAspire.Consolidation.Domain.Projections;
using DaprAspire.Consolidation.Domain.ServiceDefinitions;

using MongoDB.Driver;

namespace DaprAspire.Consolidation.Infrastructure.Repositories
{
    public class ProjectionRepository<T>(IMongoDatabase database) : IProjectionRepository<T> where T : Projection
    {
        private static readonly string _collectionName = typeof(T).Name.ToLowerInvariant() + "s";
        private readonly IMongoCollection<T> _collection = database.GetCollection<T>(_collectionName);

        public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _collection
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetByLedgerIdAsync(string ledgerId, CancellationToken cancellationToken = default)
        {
            return await _collection
                .Find(p => p.LedgerId == ledgerId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpsertAsync(T projection, CancellationToken cancellationToken = default)
        {
            projection.LastUpdated = DateTimeOffset.UtcNow.UtcDateTime;

            await _collection.ReplaceOneAsync(
                filter: p => p.Id == projection.Id,
                replacement: projection,
                options: new ReplaceOptions { IsUpsert = true },
                cancellationToken: cancellationToken
            );
        }
    }
}
