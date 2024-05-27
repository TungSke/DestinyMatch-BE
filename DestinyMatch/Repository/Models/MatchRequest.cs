using Repository.Models.Generic;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class MatchRequest : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? Status { get; set; }

    public Guid? FromId { get; set; }

    public Guid? ToId { get; set; }

    public virtual Member? From { get; set; }

    public virtual Member? To { get; set; }
}
