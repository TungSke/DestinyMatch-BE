using FPT.DestinyMatch.Repository.Models.Generic;
using System;
using System.Collections.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Verification : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? SubmittedPicture { get; set; }

    public DateTime? TimeStamp { get; set; }

    public string? Status { get; set; }

    public Guid MemberId { get; set; }

    public virtual Member Member { get; set; } = null!;
}
