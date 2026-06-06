using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class BillItem
{
    public int ItemId { get; set; }

    public int BillId { get; set; }

    public string ItemType { get; set; } = null!;

    public int ReferenceId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? SubTotal { get; set; }

    public bool? IsPaidGate { get; set; }

    public virtual Bill Bill { get; set; } = null!;
}
