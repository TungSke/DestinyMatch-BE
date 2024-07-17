using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Message : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public string? Status { get; set; }

    public Guid MatchingId { get; set; }

    public Guid SenderId { get; set; }

    public virtual Matching Matching { get; set; } = null!;

    public virtual Member Sender { get; set; } = null!;
}
