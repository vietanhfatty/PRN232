using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Authorize]
public class AppointmentsController : Controller
{
    private readonly ApiService _apiService;

    public AppointmentsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? fromDate = null, 
        DateTime? toDate = null, 
        string? status = null, 
        string? search = null)
    {
        // 1. Build OData query dynamically
        var filters = new List<string>();

        if (fromDate.HasValue)
        {
            filters.Add($"AppointmentDate ge {fromDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
        }

        if (toDate.HasValue)
        {
            filters.Add($"AppointmentDate le {toDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
        }

        if (!string.IsNullOrEmpty(status))
        {
            filters.Add($"Status eq '{status}'");
        }

        if (!string.IsNullOrEmpty(search))
        {
            // Case-insensitive filtering using standard OData functions
            var escapedSearch = Uri.EscapeDataString(search);
            filters.Add($"(contains(Patient/FullName, '{escapedSearch}') or contains(Doctor/FullName, '{escapedSearch}'))");
        }

        string path = "odata/Appointments?$expand=Patient,Doctor";
        if (filters.Any())
        {
            path += "&$filter=" + string.Join(" and ", filters);
        }

        // Fetch Data
        var appointments = await _apiService.GetAsync<List<Appointment>>(path) ?? new List<Appointment>();
        var patients = await _apiService.GetAsync<List<Patient>>("odata/Patients") ?? new List<Patient>();
        var staffs = await _apiService.GetAsync<List<Staff>>("odata/Staffs") ?? new List<Staff>();
        var medicines = await _apiService.GetAsync<List<Medicine>>("odata/Medicines") ?? new List<Medicine>();

        // Filter doctors (Staff with Position 'Bác sĩ' or Specialization configured)
        var doctors = staffs.Where(s => s.Position == "Bác sĩ" || !string.IsNullOrEmpty(s.Specialization)).ToList();

        ViewBag.Patients = patients;
        ViewBag.Doctors = doctors;
        ViewBag.Medicines = medicines;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
        ViewBag.Status = status;
        ViewBag.Search = search;

        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> Save(AppointmentDTO appointment)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            TempData["Error"] = "Validation failed: " + string.Join(" ", errors);
            return RedirectToAction("Index");
        }

        HttpResponseMessage response;
        if (appointment.AppointmentId == 0)
        {
            response = await _apiService.PostAsync("api/appointments", appointment);
        }
        else
        {
            response = await _apiService.PutAsync($"api/appointments/{appointment.AppointmentId}", appointment);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = "Appointment scheduled successfully.";
        }
        else
        {
            TempData["Error"] = "Failed to schedule appointment.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var appointment = await _apiService.GetAsync<Appointment>($"api/appointments/{id}");
        if (appointment == null)
        {
            TempData["Error"] = "Appointment not found.";
            return RedirectToAction("Index");
        }

        var dto = new AppointmentDTO
        {
            AppointmentId = appointment.AppointmentId,
            PatientId = appointment.PatientId.GetValueOrDefault(),
            DoctorId = appointment.DoctorId.GetValueOrDefault(),
            AppointmentDate = appointment.AppointmentDate,
            Reason = appointment.Reason,
            Status = status
        };

        var response = await _apiService.PutAsync($"api/appointments/{id}", dto);
        if (response.IsSuccessStatusCode)
        {
            TempData["Success"] = $"Appointment status updated to '{status}'.";
        }
        else
        {
            TempData["Error"] = "Failed to update appointment status.";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalRecord([FromBody] MedicalRecordCreateDTO recordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data provided." });
        }

        var response = await _apiService.PostAsync("api/medicalrecords", recordDto);
        if (response.IsSuccessStatusCode)
        {
            return Ok(new { message = "Medical record and prescription created successfully." });
        }

        return BadRequest(new { message = "Failed to create medical record on backend." });
    }
}
