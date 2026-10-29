using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups(
            [FromQuery] string? searchTerm,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            var groups = await _groupService.ListGroupsAsync(
                searchTerm,
                page,
                pageSize,
                cancellationToken);

            return Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(string id, CancellationToken cancellationToken)
        {
            var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
            return group is null ? NotFound() : Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateGroupCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _groupService.CreateGroupAsync(command, cancellationToken);
                return CreatedAtAction(nameof(GetGroupById), new { id = group.Id },group);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(string id, UpdateGroupCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _groupService.UpdateGroupAsync(id, command, cancellationToken);
                return group is null ? NotFound() : Ok(group);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(string id, CancellationToken cancellationToken)
        {
            var deleted = await _groupService.DeleteGroupAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
