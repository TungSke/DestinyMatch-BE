namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MemberRequest
    {
        public class DateModel
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
        }
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
    }
}
