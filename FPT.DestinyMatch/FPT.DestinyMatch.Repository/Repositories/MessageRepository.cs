using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MessageReposirory : GenericRepository<Message>, IMessageReposirory
    {
        public MessageReposirory(DestinyMatchContext _dbcontext) : base(_dbcontext)
        {
        }
    }
}
