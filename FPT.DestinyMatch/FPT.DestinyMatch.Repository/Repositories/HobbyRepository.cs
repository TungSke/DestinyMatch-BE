using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class HobbyRepository : GenericRepository<Hobby>, IHobbyReposiroty
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public HobbyRepository(DestinyMatchContext context) : base(context)
        {
        }

    }
}
