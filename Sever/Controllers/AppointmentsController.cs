using HospitalManagement.BusinessObject.DTOs;
using HospitalManagement.DataAccess.Entities;
using HospitalManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Sever.Controllers;

public class AppointmentsController : ODataController
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentsController(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_appointmentRepository.GetAppointments());
    }

    [HttpGet("/api/appointments/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return Ok(appointment);
    }

    [HttpPost("/api/appointments")]
    public async Task<IActionResult> Create([FromBody] AppointmentDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var appointment = new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            Reason = dto.Reason,
            Status = string.IsNullOrEmpty(dto.Status) ? "Pending" : dto.Status,
            CreatedAt = DateTime.Now
        };

        await _appointmentRepository.AddAppointmentAsync(appointment);

        dto.AppointmentId = appointment.AppointmentId;
        return CreatedAtAction(nameof(GetById), new { id = appointment.AppointmentId }, dto);
    }

    [HttpPut("/api/appointments/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentDTO dto)
    {
        if (id != dto.AppointmentId)
        {
            return BadRequest("Appointment ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existing = await _appointmentRepository.GetAppointmentByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.PatientId = dto.PatientId;
        existing.DoctorId = dto.DoctorId;
        existing.AppointmentDate = dto.AppointmentDate;
        existing.Reason = dto.Reason;
        existing.Status = dto.Status;

        await _appointmentRepository.UpdateAppointmentAsync(existing);
        return NoContent();
    }

    [HttpDelete("/api/appointments/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _appointmentRepository.GetAppointmentByIdAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        await _appointmentRepository.DeleteAppointmentAsync(id);
        return NoContent();
    }
}
