using Newtonsoft.Json;

namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class ConversationDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? RecentlyTime {get; set;}
        public DateTime? CreateTime {get; set;}
        public Guid ChattingMemberId {get; set;}
    }
}
