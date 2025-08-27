using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class Medication
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Strength { get; set; } = null!;

    public string Form { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual User User { get; set; } = null!;
}
