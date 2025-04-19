
using DaprAspire.Consolidation.Domain.Projections;
using DaprAspire.Consolidation.Domain.ServiceDefinitions;
using DaprAspire.Consolidation.Domain.ValueObjects;
using DaprAspire.Domain.CrossCutting.Messaging.Events;

namespace DaprAspire.Consolidation.Application.Services.ProjectionService
{
    public class EntryProjectionService(IProjectionRepository<DailyBalanceProjection> repository)
        : IEntryProjectionService
    {
        private readonly IProjectionRepository<DailyBalanceProjection> _repository = repository;

        public async Task ApplyAsync(EntryCreatedMessage message)
        {
            var date = DateOnly.FromDateTime(message.Timestamp.DateTime);
            var id = ProjectionId.FromLedgerAndDate(message.Id, date);

            var projection = new DailyBalanceProjection
            {
                Id = id.Value,
                LedgerId = message.Id,
                Date = date,
                Balance = 0,
                CreditCount = 0,
                DebitCount = 0,
                LastUpdated = message.Timestamp.UtcDateTime
            };

            await _repository.UpsertAsync(projection);
        }

        public async Task ApplyAsync(EntryCreditedMessage message)
        {
            var date = DateOnly.FromDateTime(message.Timestamp.DateTime);
            var id = ProjectionId.FromLedgerAndDate(message.Id, date);

            var projection = await _repository.GetByIdAsync(id.Value)
                              ?? new DailyBalanceProjection
                              {
                                  Id = id.Value,
                                  LedgerId = message.Id,
                                  Date = date,
                              };

            projection.ApplyCredit(message.Value);
            await _repository.UpsertAsync(projection);
        }

        public async Task ApplyAsync(EntryDebitedMessage message)
        {
            var date = DateOnly.FromDateTime(message.Timestamp.DateTime);
            var id = ProjectionId.FromLedgerAndDate(message.Id, date);

            var projection = await _repository.GetByIdAsync(id.Value)
                              ?? new DailyBalanceProjection
                              {
                                  Id = id.Value,
                                  LedgerId = message.Id,
                                  Date = date,
                              };

            projection.ApplyDebit(message.Value);
            await _repository.UpsertAsync(projection);
        }

        public async Task<DailyBalanceProjection?> GetLatestDailyBalanceAsync(string ledgerId, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByLedgerIdAsync(ledgerId, cancellationToken);
        }
    }
}
