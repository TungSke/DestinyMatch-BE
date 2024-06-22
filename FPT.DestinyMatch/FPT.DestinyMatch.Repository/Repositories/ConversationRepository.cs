using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {                                       //inheritance                implement interface
        //************************[ DECLARATION ]************************
        public ConversationRepository(DestinyMatchContext context) : base(context)
        {
        }

        //**************************[ METHODS ]**************************
        public async Task<bool> UpdateRecentlyTime(Guid conversationId)
        {
            var currentConversation = await GetByIdAsync(conversationId);
            if (currentConversation is null)
            {
                return false;
            }
            currentConversation.RecentlyActivity = DateTime.Now;
            return await SaveChangeAsync();
        }
    }
}
