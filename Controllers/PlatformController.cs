using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Dtos;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;

        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PlatformDto>>> ListPlatforms(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? page = null,
            [FromQuery] int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            var platforms = await _platformService.ListPlatformsAsync(searchTerm, page, pageSize, cancellationToken);
            return Ok(platforms);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformDto>> GetPlatformById(string id, CancellationToken cancellationToken)
        {
            var platform = await _platformService.GetPlatformByIdAsync(id, cancellationToken);

            if (platform is null)
                return NotFound();

            return Ok(platform);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformDto>> CreatePlatform([FromBody] CreatePlatformCommand request, CancellationToken cancellationToken)
        {
            var platform = await _platformService.CreatePlatformAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetPlatformById), new { id = platform.Id }, platform);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlatformDto>> UpdatePlatform(string id, [FromBody] UpdatePlatformCommand request, CancellationToken cancellationToken)
        {
            var platform = await _platformService.UpdatePlatformAsync(id, request, cancellationToken);

            if (platform is null)
                return NotFound();

            return Ok(platform);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(string id, CancellationToken cancellationToken)
        {
            var deleted = await _platformService.DeletePlatformAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}