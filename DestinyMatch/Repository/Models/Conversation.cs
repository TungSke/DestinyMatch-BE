using Repository.Models.Generic;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Conversation : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public Guid FirstMemberId { get; set; }

    public Guid SecondMemberId { get; set; }

    public virtual Member FirstMember { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Member SecondMember { get; set; } = null!;
}
