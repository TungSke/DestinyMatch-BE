using Repository.Models.Generic;
using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class MemberPackage : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? MemberId { get; set; }

    public Guid? PackageId { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Package? Package { get; set; }
}
