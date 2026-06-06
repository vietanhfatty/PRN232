using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class RoomAllocation
{
    public int AllocationId { get; set; }

    public int PatientId { get; set; }

    public int BedId { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    public virtual Bed Bed { get; set; } = null!;

    public virtual ICollection<InpatientNote> InpatientNotes { get; set; } = new List<InpatientNote>();

    public virtual Patient Patient { get; set; } = null!;
}
