using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int MedicationId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string Pattern { get; set; } = null!;

    public decimal DoseAmount { get; set; }

    public virtual ICollection<Intake> Intakes { get; set; } = new List<Intake>();

    public virtual Medication Medication { get; set; } = null!;
}
