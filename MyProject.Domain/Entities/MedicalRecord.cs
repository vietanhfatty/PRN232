using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime VisitDate { get; set; }

    public string Symptoms { get; set; } = null!;

    public string Diagnosis { get; set; } = null!;

    public string? TreatmentPlan { get; set; }

    public virtual ICollection<AdmissionOrder> AdmissionOrders { get; set; } = new List<AdmissionOrder>();

    public virtual Staff Doctor { get; set; } = null!;

    public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
