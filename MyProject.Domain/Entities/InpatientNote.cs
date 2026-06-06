using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class InpatientNote
{
    public int NoteId { get; set; }

    public int AllocationId { get; set; }

    public string? Pulse { get; set; }

    public string? Temperature { get; set; }

    public string? BloodPressure { get; set; }

    public string? DoctorNotes { get; set; }

    public string? NurseNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual RoomAllocation Allocation { get; set; } = null!;
}
