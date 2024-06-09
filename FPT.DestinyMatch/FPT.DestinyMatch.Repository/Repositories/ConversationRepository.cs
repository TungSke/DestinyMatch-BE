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
    }
}
