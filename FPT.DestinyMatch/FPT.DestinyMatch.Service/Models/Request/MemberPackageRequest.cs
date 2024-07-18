namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MemberPackageRequest
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid MemberId { get; set; }

        public Guid PackageId { get; set; }
    }
}
