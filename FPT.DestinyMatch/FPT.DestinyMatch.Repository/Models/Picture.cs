using FPT.DestinyMatch.Repository.Models.Generic;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Picture : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? UrlPath { get; set; }

    public bool? IsAvatar { get; set; }

    public string? Status { get; set; }

    public Guid MemberId { get; set; }

    [JsonIgnore]
    public virtual Member Member { get; set; } = null!;
}
