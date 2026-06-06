using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class AdmissionOrder
{
    public int OrderId { get; set; }

    public int RecordId { get; set; }

    public int DoctorId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Reason { get; set; } = null!;

    public string? Status { get; set; }

    public virtual Staff Doctor { get; set; } = null!;

    public virtual MedicalRecord Record { get; set; } = null!;
}
