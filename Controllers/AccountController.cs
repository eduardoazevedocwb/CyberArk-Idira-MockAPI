using CyberArk_Idira_MockApi.Application.Commands;
using CyberArk_Idira_MockApi.Application.Dtos;
using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AccountDto>>> ListAccounts(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? page = null,
            [FromQuery] int? pageSize = null,
            CancellationToken cancellationToken = default)
        {
            var accounts = await _accountService.ListAccountsAsync(searchTerm, page, pageSize, cancellationToken);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccountById(string id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);

            if (account is null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountService.CreateAccountAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> UpdateAccount(string id, [FromBody] UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountService.UpdateAccountAsync(id, request, cancellationToken);

            if (account is null)
                return NotFound();

            return Ok(account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id, CancellationToken cancellationToken)
        {
            var deleted = await _accountService.DeleteAccountAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}