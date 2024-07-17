using FPT.DestinyMatch.Repository.Models.Generic;

namespace FPT.DestinyMatch.Repository.Models;

public partial class Member : GenericModel<Guid>
{
    public Guid Id { get; set; }

    public string? Fullname { get; set; }

    public string? Introduce { get; set; }

    public DateOnly? Dob { get; set; }

    public bool? Gender { get; set; }

    public string? Address { get; set; }

    public int? Surplus { get; set; }

    public string? Status { get; set; }

    public Guid AccountId { get; set; }

    public Guid UniversityId { get; set; }

    public Guid MajorId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Major Major { get; set; } = null!;

    public virtual ICollection<Matching> MatchingFirstMembers { get; set; } = new List<Matching>();

    public virtual ICollection<Matching> MatchingSecondMembers { get; set; } = new List<Matching>();

    public virtual ICollection<MemberPackage> MemberPackages { get; set; } = new List<MemberPackage>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();

    public virtual University University { get; set; } = null!;

    public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
}
