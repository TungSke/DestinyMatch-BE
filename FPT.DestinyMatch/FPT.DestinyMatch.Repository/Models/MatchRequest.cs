using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class MatchRequest : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? Status { get; set; }

    public Guid FromId { get; set; }

    public Guid ToId { get; set; }

    public virtual Member From { get; set; } = null!;

    public virtual Member To { get; set; } = null!;
}
