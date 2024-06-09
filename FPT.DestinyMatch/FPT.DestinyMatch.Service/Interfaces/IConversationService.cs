using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IConversationService
    {
        public Task<Conversation?> GetConversationDetail(Guid conversationId, Guid memberUsingId);
        public Task<List<Conversation>> GetConversationList(Guid ofMemberId);
        public Task<Conversation> StartNewConversation(Guid fromMemberId, Guid toMemberId);
        public Task<bool> ChangeNameConversation(Guid conversationId, Guid interactingMemberId, string newName);
        public Task<bool> DeleteConversation(Guid conversationId);
    }
}
