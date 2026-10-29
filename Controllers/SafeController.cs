using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Dtos;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SafeController : ControllerBase
    {
        private readonly ISafeService _safeService;

        public SafeController(ISafeService safeService)
        {
            _safeService = safeService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SafeDto>>> ListSafes(
            [FromQuery] string? searchTerm = null, 
            [FromQuery] int? page = null, 
            [FromQuery] int? pageSize = null, 
            CancellationToken cancellationToken = default
        )
        {
            var safes = await _safeService.ListSafesAsync(searchTerm, page, pageSize, cancellationToken);
            return Ok(safes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SafeDto>> GetSafeById(string id, CancellationToken cancellationToken)
        {
            var safe = await _safeService.GetSafeByIdAsync(id, cancellationToken);

            if (safe is null)
                return NotFound();

            return Ok(safe);
        }

        [HttpPost]
        public async Task<ActionResult<SafeDto>> CreateSafe([FromBody] CreateSafeCommand request, CancellationToken cancellationToken)
        {
            var safe = await _safeService.CreateSafeAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetSafeById), new { id = safe.Id }, safe);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SafeDto>> UpdateSafe(string id, [FromBody] UpdateSafeCommand request, CancellationToken cancellationToken)
        {
            var safe = await _safeService.UpdateSafeAsync(id, request, cancellationToken);

            if (safe is null)
                return NotFound();

            return Ok(safe);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSafe(string id, CancellationToken cancellationToken)
        {
            var deleted = await _safeService.DeleteSafeAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
