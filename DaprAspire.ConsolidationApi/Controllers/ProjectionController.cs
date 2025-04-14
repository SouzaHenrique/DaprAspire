using DaprAspire.Consolidation.Application.Services.ProjectionService;

using Microsoft.AspNetCore.Mvc;

namespace DaprAspire.ConsolidationApi.Controllers
{
    [ApiController]
    [Route("projections")]
    public class ProjectionController(IEntryProjectionService projectionService) : ControllerBase
    {
        private readonly IEntryProjectionService _projectionService = projectionService;

        [HttpGet("daily/{ledgerId}")]
        public async Task<IActionResult> GetLatestDailyBalance(string ledgerId)
        {
            var projection = await _projectionService.GetLatestDailyBalanceAsync(ledgerId);

            return projection is null
                ? NotFound()
                : Ok(projection);
        }
    }
}
