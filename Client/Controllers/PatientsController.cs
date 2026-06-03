using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Authorize]
public class PatientsController : Controller
{
    private readonly ApiService _apiService;

    public PatientsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search = null)
    {
        string path = "odata/Patients";
        if (!string.IsNullOrEmpty(search))
        {
            path += $"?$filter=contains(FullName, '{Uri.EscapeDataString(search)}')";
        }

        var patientsResponse = await _apiService.GetAsync<ODataResponse<List<Patient>>>(path);
        var patients = patientsResponse?.Value;
        ViewData["Search"] = search;
        return View(patients ?? new List<Patient>());
    }

    [HttpPost]
    public async Task<IActionResult> Save(PatientDTO patient)
    {
        if (!ModelState.IsValid)
        {
            // Gather model state errors and store in TempData
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["Error"] = "Validation failed: " + string.Join(" ", errors);
            return RedirectToAction("Index");
        }

        HttpResponseMessage response;
        if (patient.PatientId == 0)
        {
            // Create
            response = await _apiService.PostAsync("api/patients", patient);
        }
        else
        {
            // Update
            response = await _apiService.PutAsync($"api/patients/{patient.PatientId}", patient);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Patient saved successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to save patient. Please check data constraints.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var response = await _apiService.DeleteAsync($"api/patients/{id}");
        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Patient deleted successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to delete patient.";
        }

        return RedirectToAction("Index");
    }
}
