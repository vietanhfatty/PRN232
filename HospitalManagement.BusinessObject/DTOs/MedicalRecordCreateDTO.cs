using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.BusinessObject.DTOs;

public class MedicalRecordCreateDTO
{
    [Required(ErrorMessage = "Appointment ID is required.")]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = "Diagnosis is required.")]
    [StringLength(500, ErrorMessage = "Diagnosis must not exceed 500 characters.")]
    public string Diagnosis { get; set; } = null!;

    [StringLength(1000, ErrorMessage = "Treatment Plan must not exceed 1000 characters.")]
    public string? TreatmentPlan { get; set; }

    [StringLength(500, ErrorMessage = "Doctor Notes must not exceed 500 characters.")]
    public string? DoctorNotes { get; set; }

    public List<PrescriptionItemDTO> Prescriptions { get; set; } = new();
}
