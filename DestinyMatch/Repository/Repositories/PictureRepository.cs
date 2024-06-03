using Repository.Interfaces;
using Repository.Models;

namespace Repository.Repositories
{
    public class PictureRepository : GenericRepository<Picture>, IPictureRepository
    {
        public PictureRepository(DestinyMatchContext context) : base(context)
        {
        }
    }
}
