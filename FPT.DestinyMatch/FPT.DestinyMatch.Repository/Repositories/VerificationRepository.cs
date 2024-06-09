using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class VerificationRepository : GenericRepository<Verification>, IVerificationRepository
    {
        public VerificationRepository(DestinyMatchContext context) : base(context)
        {
        }
    }
}