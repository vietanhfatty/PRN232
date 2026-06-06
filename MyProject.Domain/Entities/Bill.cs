using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Bill
{
    public int BillId { get; set; }

    public int PatientId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal InsuranceDiscount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal NetAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();

    public virtual Patient Patient { get; set; } = null!;
}
