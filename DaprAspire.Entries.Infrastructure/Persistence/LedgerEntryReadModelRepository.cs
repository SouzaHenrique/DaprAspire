using DaprAspire.Entries.Domain.ReadModels;
using DaprAspire.Entries.Domain.ServiceDefinitions.Persistence;

using MongoDB.Driver;

namespace DaprAspire.Entries.Infrastructure.Persistence
{
    public class LedgerEntryReadModelRepository : ILedgerEntryReadModelRepository
    {
        private readonly IMongoCollection<LedgerEntryReadModel> _collection;

        public LedgerEntryReadModelRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<LedgerEntryReadModel>("eventflow-ledgerentryreadmodel");
        }

        public async Task<IReadOnlyList<LedgerEntryReadModel>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var skip = (pageNumber - 1) * pageSize;

            var result = await _collection
                .Find(FilterDefinition<LedgerEntryReadModel>.Empty)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.CountDocumentsAsync(FilterDefinition<LedgerEntryReadModel>.Empty, cancellationToken: cancellationToken);
        }
    }
}
