using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Dtos;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SafeMemberController : ControllerBase
    {
        private readonly ISafeMemberService _safeMemberService;

        public SafeMemberController(ISafeMemberService safeMemberService)
        {
            _safeMemberService = safeMemberService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SafeMemberDto>>> ListSafeMembers(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? safeId = null,
            [FromQuery] int? page = null,
            [FromQuery] int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            var members = await _safeMemberService.ListSafeMembersAsync(searchTerm, safeId, page, pageSize, cancellationToken);
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SafeMemberDto>> GetSafeMemberById(string id, CancellationToken cancellationToken)
        {
            var member = await _safeMemberService.GetSafeMemberByIdAsync(id, cancellationToken);

            if (member is null)
                return NotFound();

            return Ok(member);
        }

        [HttpPost]
        public async Task<ActionResult<SafeMemberDto>> CreateSafeMember([FromBody] CreateSafeMemberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var member = await _safeMemberService.CreateSafeMemberAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetSafeMemberById), new { id = member.Id }, member);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Safe '{request.SafeId}' was not found.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SafeMemberDto>> UpdateSafeMember(string id, [FromBody] UpdateSafeMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _safeMemberService.UpdateSafeMemberAsync(id, request, cancellationToken);

            if (member is null)
                return NotFound();

            return Ok(member);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSafeMember(string id, CancellationToken cancellationToken)
        {
            var deleted = await _safeMemberService.DeleteSafeMemberAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}