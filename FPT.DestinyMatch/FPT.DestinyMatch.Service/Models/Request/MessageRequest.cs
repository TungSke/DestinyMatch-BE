namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MessageRequest
    {
        public string Content { get; set; } = null!;
        public string? Status { get; set; }

        public Guid ConversationId { get; set; }

        public Guid SenderId { get; set; }
    }
}
