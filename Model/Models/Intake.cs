using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Intake
{
    public int Id { get; set; }

    public int ScheduleId { get; set; }

    public DateOnly PlannedAt { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly TakenAt { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;
}
