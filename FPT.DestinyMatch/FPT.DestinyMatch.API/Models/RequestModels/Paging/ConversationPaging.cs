namespace FPT.DestinyMatch.API.Models.RequestModels.Paging
{
    public class ConversationPaging
    {
        public int Amount { get; set; }
        public int Page { get; set; }
        public Guid CurrentUsingMemberId {  get; set; }
        public string? NameKeyword { get; set; }
        public string? Status { get; set; }
        public bool OrderByDescending { get; set; } = true;
    }
}
