using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class VerificationRepository : GenericRepository<Verification>, IVerificationRepository
    {
        public VerificationRepository(DestinyMatchContext context) : base(context)
        {
        }

        public async Task<Verification?> GetDetailAsync(Guid verificationId)
        {
            //return await DMDB.Verifications
            //              .Include(v => v.Member)
            //              .FirstOrDefaultAsync(v => v.Id == verificationId);
            return null;
        }
        public async Task<IEnumerable<Verification>> GetListVerificationAsync(
            int amountItem, int pageIndex,
            Guid memberId,
            string? statusSearch,
            bool sortAscending)
        {

            return null;
        }
    }
}