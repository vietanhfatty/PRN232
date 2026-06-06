using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public string? InsuranceNo { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
