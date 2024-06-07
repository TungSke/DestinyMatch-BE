using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        public UniversityRepository(DestinyMatchContext context) : base(context)
        {
        }
    }
}
