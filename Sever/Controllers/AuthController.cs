using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sever.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IStaffRepository _staffRepository;

    public AuthController(IAccountRepository accountRepository, IStaffRepository staffRepository)
    {
        _accountRepository = accountRepository;
        _staffRepository = staffRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var account = await _accountRepository.GetAccounts()
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == request.Username);

        if (account == null || !account.IsActive.GetValueOrDefault(true))
        {
            return Unauthorized(new { message = "Invalid username or inactive account." });
        }

        // Support plain text checking and FPT seed data formats:
        // e.g. "hashed_password_123456" for password "123456"
        bool isPasswordValid = account.PasswordHash == request.Password || 
                              account.PasswordHash == $"hashed_password_{request.Password}";

        if (!isPasswordValid)
        {
            return Unauthorized(new { message = "Invalid password." });
        }

        var staff = await _staffRepository.GetStaffs()
            .FirstOrDefaultAsync(s => s.AccountId == account.AccountId);

        var session = new UserSession
        {
            AccountId = account.AccountId,
            Username = account.Username,
            RoleName = account.Role?.RoleName ?? "Staff",
            Email = account.Email,
            FullName = staff?.FullName ?? account.Username
        };

        return Ok(session);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logged out successfully." });
    }
}
