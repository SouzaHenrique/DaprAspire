using DaprAspire.Consolidation.Application.Services.ProjectionService;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    [Route("projections")]
    [Authorize]
    public class ProjectionController(IEntryProjectionService projectionService, ILogger<ProjectionController> logger) : ControllerBase
    {
        private readonly IEntryProjectionService _projectionService = projectionService;
        private readonly ILogger<ProjectionController> _logger = logger;

        [HttpGet("daily/{ledgerId}")]
        public async Task<IActionResult> GetLatestDailyBalance(string ledgerId)
        {
            _logger.LogInformation("Fetching latest daily balance projection for ledger ID: {LedgerId}", ledgerId);
            var projection = await _projectionService.GetLatestDailyBalanceAsync(ledgerId);

            return projection is null
                ? NotFound()
                : Ok(projection);
        }
    }
}
