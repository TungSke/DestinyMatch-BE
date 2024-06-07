using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MajorRepository : GenericRepository<Major>, IMajorRepository
    {                               //inheritance               implement interface
        //************************[ DECLARATION ]************************
        public MajorRepository(DestinyMatchContext context) : base(context)
        {
        }

    }
}
