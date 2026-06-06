using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public decimal DailyRate { get; set; }

    public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();
}
