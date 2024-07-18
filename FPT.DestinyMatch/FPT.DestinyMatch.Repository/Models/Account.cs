using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Account : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? FcmtToken { get; set; }

    public bool ReceiveNotifiEmail { get; set; }

    public DateTime? CreateAt { get; set; }

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Member? Member { get; set; }
}
