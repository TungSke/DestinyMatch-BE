using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {                                       //inheritance                implement interface
        //************************[ DECLARATION ]************************
        public ConversationRepository(DestinyMatchContext context) : base(context)
        {
        }

        //**************************[ METHODS ]**************************
        public async Task<bool> UpdateRecentlyTimeAsync(Guid conversationId)
        {
            var currentConversation = await GetByIdAsync(conversationId);
            if (currentConversation is null)
            {
                return false;
            }
            currentConversation.RecentlyActivity = DateTime.Now;
            return await SaveChangeAsync();
        }
        public async Task<IEnumerable<Conversation>> GetRecentlyListAsync(Guid memberId, int pageIndex)
        {
            //var query = DMDB.Conversations.AsQueryable();

            ////Apply filter
            //query = query.Where(con => con.FirstMemberId == memberId || con.SecondMemberId == memberId);

            ////Sort order by recently
            //query = query.OrderByDescending(con => con.RecentlyActivity);

            //// Apply paging 
            //var pagedConversations = await query
            //    .Skip((pageIndex - 1) * 10)
            //    .Take(10)
            //    .Select(con => new Conversation
            //    {
            //        Id = con.Id,
            //        FirstName = con.FirstName,
            //        SecondName = con.SecondName,
            //        RecentlyActivity = con.RecentlyActivity,
            //        CreatedAt = con.CreatedAt,
            //        Status = con.Status,
            //        FirstMemberId = con.FirstMemberId,
            //        SecondMemberId = con.SecondMemberId
            //        //Skip Virtual Object of EF navigate to relevant Model
            //    })
            //    .ToListAsync();
            //pageIndex += 1;
            //return pagedConversations;
            return null;
        }
        public async Task<IEnumerable<Conversation>> GetFilteredListAsync(int amountItem, int pageIndex, Guid memberUsingId,
            string? keyword, string? statusSearch, bool isDescending)
        {
            //var query = DMDB.Conversations.AsQueryable();

            //// Apply search
            //query = query.Where(con => con.FirstMemberId == memberUsingId);
            //query = query.Where(con => con.SecondMemberId == memberUsingId);

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(con => con.SecondName.ToLower().Contains(keyword.ToLower()));
            //    query = query.Where(con => con.FirstName.ToLower().Contains(keyword.ToLower()));
            //}

            //if (!string.IsNullOrEmpty(statusSearch))
            //{
            //    query = query.Where(con => con.Status.ToLower().Equals(statusSearch.ToLower()));
            //}

            //// Sort by date recently
            //query = isDescending ?
            //    query.OrderByDescending(con => con.RecentlyActivity) : query.OrderBy(con => con.RecentlyActivity);


            //// Apply paging
            //var pagedConversations = await query
            //    .Skip((pageIndex - 1) * amountItem)
            //    .Take(amountItem)
            //    .ToListAsync();

            //return pagedConversations;
            return null;
        }
    }
}
