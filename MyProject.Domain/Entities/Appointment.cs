using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Type { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public int? QueueNumber { get; set; }

    public virtual Staff Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
