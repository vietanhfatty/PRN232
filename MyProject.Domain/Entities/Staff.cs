using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Specialization { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AdmissionOrder> AdmissionOrders { get; set; } = new List<AdmissionOrder>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual Role Role { get; set; } = null!;
}
