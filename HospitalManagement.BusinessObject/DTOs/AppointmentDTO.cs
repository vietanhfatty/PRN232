using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.BusinessObject.DTOs;

public class AppointmentDTO
{
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "Patient is required.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Doctor is required.")]
    public int DoctorId { get; set; }

    [Required(ErrorMessage = "Appointment Date is required.")]
    public DateTime AppointmentDate { get; set; }

    [StringLength(500, ErrorMessage = "Reason must not exceed 500 characters.")]
    public string? Reason { get; set; }

    [StringLength(50, ErrorMessage = "Status must not exceed 50 characters.")]
    public string Status { get; set; } = "Pending";
}
