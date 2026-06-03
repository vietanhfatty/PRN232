using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class PatientsController : ODataController
{
    private readonly IPatientRepository _patientRepository;

    public PatientsController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_patientRepository.GetPatients());
    }

    [HttpGet("/api/patients/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        return Ok(patient);
    }

    [HttpPost("/api/patients")]
    public async Task<IActionResult> Create([FromBody] PatientDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var patient = new Patient
        {
            FullName = dto.FullName,
            Gender = dto.Gender,
            DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            HealthInsuranceCode = dto.HealthInsuranceCode,
            CreatedAt = DateTime.Now
        };

        await _patientRepository.AddPatientAsync(patient);
        
        dto.PatientId = patient.PatientId;
        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, dto);
    }

    [HttpPut("/api/patients/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientDTO dto)
    {
        if (id != dto.PatientId)
        {
            return BadRequest("Patient ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = await _patientRepository.GetPatientByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.FullName = dto.FullName;
        existing.Gender = dto.Gender;
        existing.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth);
        existing.PhoneNumber = dto.PhoneNumber;
        existing.Address = dto.Address;
        existing.HealthInsuranceCode = dto.HealthInsuranceCode;

        await _patientRepository.UpdatePatientAsync(existing);
        return NoContent();
    }

    [HttpDelete("/api/patients/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _patientRepository.GetPatientByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        await _patientRepository.DeletePatientAsync(id);
        return NoContent();
    }
}
