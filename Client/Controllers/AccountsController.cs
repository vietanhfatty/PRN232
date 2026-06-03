using HospitalManagement.DataAccess.Entities;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Authorize(Roles = "Admin")]
public class AccountsController : Controller
{
    private readonly ApiService _apiService;

    public AccountsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await _apiService.GetAsync<List<Account>>("odata/Accounts?$expand=Role");
        var roles = await _apiService.GetAsync<List<Role>>("api/roles");

        ViewBag.Roles = roles ?? new List<Role>();
        return View(accounts ?? new List<Account>());
    }

    [HttpPost]
    public async Task<IActionResult> Save(Account account)
    {
        // Simple manual validation just in case
        if (string.IsNullOrEmpty(account.Username))
        {
            TempData["Error"] = "Username is required.";
            return RedirectToAction("Index");
        }

        HttpResponseMessage response;
        if (account.AccountId == 0)
        {
            // Create
            response = await _apiService.PostAsync("api/accounts", account);
        }
        else
        {
            // Update
            response = await _apiService.PutAsync($"api/accounts/{account.AccountId}", account);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Account saved successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to save account. Username or Email may already exist.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _apiService.DeleteAsync($"api/accounts/{id}");
        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Account deleted successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to delete account.";
        }

        return RedirectToAction("Index");
    }
}
