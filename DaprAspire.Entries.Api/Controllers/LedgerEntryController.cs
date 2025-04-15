using DaprAspire.Domain.CrossCutting.DTO.Entries;
using DaprAspire.Entries.Domain.Aggregates;
using DaprAspire.Entries.Domain.Commands;
using DaprAspire.Entries.Domain.Queries;

using EventFlow;
using EventFlow.Queries;

using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.Entries.Api.Controllers
{
    [ApiController]
    [Route("entries")]
    public class LedgerEntryController(ICommandBus commandBus, IQueryProcessor queryProcessor) : ControllerBase
    {
        private readonly ICommandBus _commandBus = commandBus;
        private readonly IQueryProcessor _queryProcessor = queryProcessor;

        [HttpGet("{id}")]
        public IActionResult GetId(string id) => Ok(new { id });

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var id = LedgerEntryId.New;

            await _commandBus.PublishAsync(new CreateEntryCommand(id), CancellationToken.None);

            return CreatedAtAction(nameof(GetId), new { id = id.Value }, new { id = id.Value });
        }

        [HttpPost("{id}/credit")]
        public async Task<IActionResult> Credit(string id, [FromBody] ValueRequest request)
        {
            var entryId = new LedgerEntryId(id);

            await _commandBus.PublishAsync(new CreditEntryCommand(entryId, request.Value), CancellationToken.None);

            return Ok();
        }

        [HttpPost("{id}/debit")]
        public async Task<IActionResult> Debit(string id, [FromBody] ValueRequest request)
        {
            var entryId = new LedgerEntryId(id);

            await _commandBus.PublishAsync(new DebitEntryCommand(entryId, request.Value), CancellationToken.None);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] LedgerEntryQueryRequest request)
        {
            var query = new GetAllLedgerEntriesQuery(request.PageNumber, request.PageSize);
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            return Ok(result);
        }
    }
}
