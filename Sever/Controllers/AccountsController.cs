using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class AccountsController : ODataController
{
    private readonly IAccountRepository _accountRepository;

    public AccountsController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_accountRepository.GetAccounts());
    }

    [HttpGet("/api/accounts/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var account = await _accountRepository.GetAccountByIdAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost("/api/accounts")]
    public async Task<IActionResult> Create([FromBody] Account account)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Apply hash format to input passwords to maintain consistency
        if (string.IsNullOrEmpty(account.PasswordHash))
        {
            account.PasswordHash = "hashed_password_123456";
        }
        else if (!account.PasswordHash.StartsWith("hashed_password_"))
        {
            account.PasswordHash = $"hashed_password_{account.PasswordHash}";
        }

        account.CreatedAt = DateTime.Now;

        await _accountRepository.AddAccountAsync(account);
        return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, account);
    }

    [HttpPut("/api/accounts/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Account account)
    {
        if (id != account.AccountId)
        {
            return BadRequest("Account ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = await _accountRepository.GetAccountByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Username = account.Username;
        if (!string.IsNullOrEmpty(account.PasswordHash))
        {
            if (!account.PasswordHash.StartsWith("hashed_password_"))
            {
                existing.PasswordHash = $"hashed_password_{account.PasswordHash}";
            }
            else
            {
                existing.PasswordHash = account.PasswordHash;
            }
        }
        existing.Email = account.Email;
        existing.IsActive = account.IsActive;
        existing.RoleId = account.RoleId;

        await _accountRepository.UpdateAccountAsync(existing);
        return NoContent();
    }

    [HttpDelete("/api/accounts/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _accountRepository.GetAccountByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        await _accountRepository.DeleteAccountAsync(id);
        return NoContent();
    }
}
