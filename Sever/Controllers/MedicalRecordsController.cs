using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class MedicalRecordsController : ODataController
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public MedicalRecordsController(
        IMedicalRecordRepository medicalRecordRepository, 
        IAppointmentRepository appointmentRepository)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _appointmentRepository = appointmentRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_medicalRecordRepository.GetMedicalRecords());
    }

    [HttpGet("/api/medicalrecords/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _medicalRecordRepository.GetMedicalRecordByIdAsync(id);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    [HttpPost("/api/medicalrecords")]
    public async Task<IActionResult> Create([FromBody] MedicalRecordCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verify the appointment exists
        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(dto.AppointmentId);
        if (appointment == null)
        {
            return BadRequest(new { message = "Appointment not found." });
        }

        // Create the Medical Record entity
        var record = new MedicalRecord
        {
            AppointmentId = dto.AppointmentId,
            Diagnosis = dto.Diagnosis,
            TreatmentPlan = dto.TreatmentPlan,
            DoctorNotes = dto.DoctorNotes,
            UpdatedAt = DateTime.Now
        };

        // Create Prescription entities
        foreach (var item in dto.Prescriptions)
        {
            record.Prescriptions.Add(new Prescription
            {
                MedicineId = item.MedicineId,
                Quantity = item.Quantity,
                Dosage = item.Dosage
            });
        }

        // Save Medical Record (and cascade prescriptions)
        await _medicalRecordRepository.AddMedicalRecordAsync(record);

        // Update Appointment status to Completed
        appointment.Status = "Completed";
        await _appointmentRepository.UpdateAppointmentAsync(appointment);

        return CreatedAtAction(nameof(GetById), new { id = record.RecordId }, record);
    }
}
