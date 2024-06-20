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
            return await DMDB.Verifications
                          .Include(v => v.Member)
                          .FirstOrDefaultAsync(v => v.Id == verificationId);
        }
        public async Task<IEnumerable<Verification>> GetListVerificationAsync(
            int amountItem, int pageIndex,
            Guid memberId,
            string? statusSearch,
            bool sortAscending)
        {
            var query = DMDB.Verifications.AsQueryable();

            // Apply search
            if (memberId != Guid.Empty)
            {
                query = query.Where(ver => ver.MemberId == memberId);
            }

            if (!statusSearch.IsNullOrEmpty())
            {
                query = query.Where(ver => ver.Status.ToLower().Contains(statusSearch.ToLower()));
            }

            //true: Oldest -> Latest
            query = sortAscending == true ?
                    query.OrderBy(ver => ver.TimeStamp) : query.OrderByDescending(acc => acc.TimeStamp);

            // Apply paging
            var pagedVerifications = await query
                .Skip((pageIndex - 1) * amountItem)
                .Take(amountItem)
                .ToListAsync();

            return pagedVerifications;
        }
    }
}