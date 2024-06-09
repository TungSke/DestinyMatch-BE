using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class ConversationDetail
    {
        [JsonProperty("id")] public Guid Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("create-time")] public DateTime? CreateTime {get; set;}
        [JsonProperty("chatting-member")] public Guid ChattingMember {get; set;}
    }
}
