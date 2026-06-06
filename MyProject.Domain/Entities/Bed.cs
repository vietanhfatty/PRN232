using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Bed
{
    public int BedId { get; set; }

    public int RoomId { get; set; }

    public string BedNumber { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<RoomAllocation> RoomAllocations { get; set; } = new List<RoomAllocation>();
}
