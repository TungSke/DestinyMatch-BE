using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Major : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
