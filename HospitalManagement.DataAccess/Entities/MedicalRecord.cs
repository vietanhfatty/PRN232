using System;
using System.Collections.Generic;

namespace HospitalManagement.DataAccess.Entities;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public int? AppointmentId { get; set; }

    public string Diagnosis { get; set; } = null!;

    public string? TreatmentPlan { get; set; }

    public string? DoctorNotes { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
