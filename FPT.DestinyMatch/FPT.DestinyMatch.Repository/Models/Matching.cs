using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Matching : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? SecondName { get; set; }

    public DateTime? RecentlyActivity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public Guid FirstMemberId { get; set; }

    public Guid SecondMemberId { get; set; }

    public virtual Member FirstMember { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Member SecondMember { get; set; } = null!;
}
