using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class LabResult
{
    public int ResultId { get; set; }

    public int RecordId { get; set; }

    public int TestId { get; set; }

    public DateTime TestDate { get; set; }

    public string ResultSummary { get; set; } = null!;

    public string? AttachmentUrl { get; set; }

    public virtual MedicalRecord Record { get; set; } = null!;

    public virtual LabTest Test { get; set; } = null!;
}
