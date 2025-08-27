using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Timezone { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }

    public virtual ICollection<Medication> Medications { get; set; } = new List<Medication>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
