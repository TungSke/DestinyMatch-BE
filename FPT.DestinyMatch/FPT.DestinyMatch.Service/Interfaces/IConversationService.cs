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
        public Task<Conversation?> GetConversationDetailAsync(Guid conversationId, Guid memberUsingId);
        public Task<IEnumerable<Conversation>> GetRecentlyConversationListAsync(Guid memberId, int pageIndex);
        public Task<IEnumerable<Conversation>> SearchConversationsListAsync(int size, int page, Guid memberUsingId,
            string? keyword, string? status, bool isDescending);
        public Task<bool> StartNewConversationAsync(Member fromMember, Guid toMemberId);
        public Task<bool> ChangeNameConversationAsync(Guid conversationId, Guid interactingMemberId, string newName);
        public Task<bool> DeleteConversationAsync(Guid conversationId);
    }
}
