using Repository.Models.Generic;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Package : GenericModel
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<MemberPackage> MemberPackages { get; set; } = new List<MemberPackage>();
}
