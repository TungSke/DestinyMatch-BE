using FPT.DestinyMatch.Repository.Models.Generic;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FPT.DestinyMatch.Repository.Models;

public partial class University : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
