using System.Text.Json.Serialization;

namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MessageRequest
    {
        public string Content { get; set; } = null!;
        [JsonIgnore]
        public DateTime? SentAt { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; }

        public Guid MatchId { get; set; }

        public Guid SenderId { get; set; }
    }
}
