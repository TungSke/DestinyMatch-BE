using System.Text.Json.Serialization;

namespace FPT.DestinyMatch.Service.Models.Request
{
    public class MatchingRequest
    {
        [JsonIgnore]
        public Guid thisMemberId { get; set; }

        public Guid toMemberId { get; set; }
    }
}
