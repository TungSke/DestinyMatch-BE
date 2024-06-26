using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Package : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public int? Duration { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<MemberPackage> MemberPackages { get; set; } = new List<MemberPackage>();
}
