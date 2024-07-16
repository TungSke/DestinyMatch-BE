using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MatchingRepository : GenericRepository<Matching>, IMatchingRepository
    {
        public MatchingRepository(DestinyMatchContext context) : base(context)
        {
        }


        public async Task<List<Matching>> GetMatchings(Guid usingMemid, int pageIndex, int pageSize, string search, string status)
        {
            var query = GetAsync().Include(x => x.Messages).Include(fm => fm.FirstMember).ThenInclude(fm => fm.Pictures).Include(sm => sm.SecondMember).ThenInclude(sm => sm.Pictures)
                .Where(x => x.Status.ToLower().Equals(status.ToLower()) && (x.FirstMemberId.Equals(usingMemid) || x.SecondMemberId.Equals(usingMemid))
                );
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FirstName.ToLower().Contains(search) || x.SecondName.ToLower().Contains(search.ToLower()));
            }
            var matchings = await query.Skip((pageIndex -1) * pageSize).Take(pageSize).ToListAsync();

            return matchings;
        }


    }
}
