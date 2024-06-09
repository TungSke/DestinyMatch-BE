namespace FPT.DestinyMatch.API.Models.ResponseModels
{
    public class ConversationDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Create_time {get; set;}
        public Guid Chatting_Member {get; set;}
    }
}
