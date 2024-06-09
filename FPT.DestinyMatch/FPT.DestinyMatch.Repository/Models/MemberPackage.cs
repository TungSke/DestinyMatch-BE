using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class MemberPackage : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public Guid MemberId { get; set; }

    public Guid PackageId { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;
}
