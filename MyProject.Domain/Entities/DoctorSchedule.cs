using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class DoctorSchedule
{
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly WorkDate { get; set; }

    public string ShiftName { get; set; } = null!;

    public int? MaxPatients { get; set; }

    public virtual Staff Doctor { get; set; } = null!;
}
