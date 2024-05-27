using Repository.Models.Generic;
using Repository.Repositories;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Account : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreateAt { get; set; }

    public string Role { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
