using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Models.Response
{
    public class MatchingResponse
    {
        public Guid ConversationId { get; set; }
        public string? ParticipantFullName { get; set; }
        public string? ParticipantAvatarUrl { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }
}
