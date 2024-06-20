namespace FPT.DestinyMatch.API.Models.RequestModels.Paging
{
    public class VerificationPaging
    {
        public int Amount { get; set; }
        public int Page { get; set; }
        public Guid MemberId { get; set; } = Guid.Empty;
        public string? Status { get; set; }
        public bool OrderByAscending { get; set; } = true;
    }
}
