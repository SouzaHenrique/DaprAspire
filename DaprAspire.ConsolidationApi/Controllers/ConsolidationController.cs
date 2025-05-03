using Microsoft.AspNetCore.Mvc;
using Dapr;
using DaprAspire.Domain.CrossCutting.Messaging.Events;
using DaprAspire.Consolidation.Application.Services.ProjectionService;
using Microsoft.AspNetCore.Authorization;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    [Route("events/entries")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    public class ConsolidationController(IEntryProjectionService projectionService, ILogger<ConsolidationController> logger) : ControllerBase
    {
        private readonly IEntryProjectionService _projectionService = projectionService;
        private readonly ILogger<ConsolidationController> _logger = logger;

        [Topic("pubsub", "entries.created")]
        [HttpPost("created")]
        public async Task<IActionResult> HandleCreated([FromBody] EntryCreatedMessage message)
        {
            _logger.LogInformation("Handling entry created event for ID: {Id}", message.Id);
            await _projectionService.ApplyAsync(message);
            return Ok();
        }

        [Topic("pubsub", "entries.credited")]
        [HttpPost("credited")]
        public async Task<IActionResult> HandleCredited([FromBody] EntryCreditedMessage message)
        {
            _logger.LogInformation("Handling entry credited event for ID: {Id}", message.Id);
            await _projectionService.ApplyAsync(message);
            return Ok();
        }

        [Topic("pubsub", "entries.debited")]
        [HttpPost("debited")]
        public async Task<IActionResult> HandleDebited([FromBody] EntryDebitedMessage message)
        {
            _logger.LogInformation("Handling entry debited event for ID: {Id}", message.Id);
            await _projectionService.ApplyAsync(message);
            return Ok();
        }        
    }
}
