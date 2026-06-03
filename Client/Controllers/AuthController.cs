using System.Security.Claims;
using HospitalManagement.BusinessObject.DTOs;
using Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _apiService;

    public AuthController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Accounts");
            }
            return RedirectToAction("Index", "Patients");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var response = await _apiService.PostAsync("api/auth/login", request);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(request);
        }

        var session = await response.Content.ReadFromJsonAsync<UserSession>();
        if (session == null)
        {
            ModelState.AddModelError("", "Failed to retrieve session from server.");
            return View(request);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, session.Username),
            new Claim(ClaimTypes.Role, session.RoleName),
            new Claim("AccountId", session.AccountId.ToString()),
            new Claim("StaffId", session.StaffId?.ToString() ?? ""),
            new Claim("FullName", session.FullName ?? session.Username),
            new Claim("Email", session.Email ?? "")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        HttpContext.Session.SetString("AccountId", session.AccountId.ToString());
        HttpContext.Session.SetString("StaffId", session.StaffId?.ToString() ?? "");
        HttpContext.Session.SetString("Username", session.Username);
        HttpContext.Session.SetString("RoleName", session.RoleName);
        HttpContext.Session.SetString("FullName", session.FullName ?? session.Username);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        if (session.RoleName == "Admin")
        {
            return RedirectToAction("Index", "Accounts");
        }
        else
        {
            return RedirectToAction("Index", "Patients");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        try
        {
            await _apiService.PostAsync<object>("api/auth/logout", new { });
        }
        catch { }
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
