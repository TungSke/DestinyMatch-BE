using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        public Task<bool> UpdateRecentlyTimeAsync(Guid conversationId);
        public Task<IEnumerable<Conversation>> GetRecentlyListAsync(Guid memberId, int pageIndex);
        public Task<IEnumerable<Conversation>> GetFilteredListAsync(int amountItem, int pageIndex, Guid memberUsingId,
            string? keyword, string? statusSearch, bool isDescending);
    }
}
