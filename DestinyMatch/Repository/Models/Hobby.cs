using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Hobby
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
