using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessages();
        Task<Message?> GetMessageById(Guid id);
        Task<bool> DeleteMessage(Guid memberId);
        Task<Message> CreateMessage(MessageRequest messageRequest);
        Task<Message> UpdateMessage(Guid Id, MessageRequest messageRequest);
        Task<IEnumerable<Message>> GetMessagesByConversationId(Guid conversationId);
    }
}
