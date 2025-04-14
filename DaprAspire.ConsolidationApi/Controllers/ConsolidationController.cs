using Microsoft.AspNetCore.Mvc;
using Dapr;
using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Consolidation.Application.Services.ProjectionService;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    [Route("events/entries")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConsolidationController(IEntryProjectionService projectionService) : ControllerBase
    {
        private readonly IEntryProjectionService _projectionService = projectionService;

        [Topic("pubsub", "entries.created")]
        [HttpPost("created")]
        public async Task<IActionResult> HandleCreated([FromBody] EntryCreatedMessage message)
        {
            await _projectionService.ApplyAsync(message);
            return Ok();
        }

        [Topic("pubsub", "entries.credited")]
        [HttpPost("credited")]
        public async Task<IActionResult> HandleCredited([FromBody] EntryCreditedMessage message)
        {
            await _projectionService.ApplyAsync(message);
            return Ok();
        }

        [Topic("pubsub", "entries.debited")]
        [HttpPost("debited")]
        public async Task<IActionResult> HandleDebited([FromBody] EntryDebitedMessage message)
        {
            await _projectionService.ApplyAsync(message);
            return Ok();
        }        
    }
}
