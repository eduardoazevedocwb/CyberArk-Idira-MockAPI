using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? searchTerm,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            var users = await _userService.ListUsersAsync(
                searchTerm,
                page,
                pageSize,
                cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.CreateUserAsync(command, cancellationToken);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
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
        public async Task<IActionResult> UpdateUser(string id, UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, command, cancellationToken);
                return user is null ? NotFound() : Ok(user);
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
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            var deleted = await _userService.DeleteUserAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
