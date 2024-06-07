using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class MatchRequestRepository: GenericRepository<MatchRequest>, IMatchRequestRepository
    {
        public MatchRequestRepository(DestinyMatchContext context):base(context)
        {
            
        }

        public async Task<List<MatchRequest>> MatchRequestOfMe(Guid fromMemberId)
        {
            return await DMDB.Set<MatchRequest>()
                .Where(mr => mr.FromId.Equals(fromMemberId))
                .ToListAsync();
        }
        public async Task<List<MatchRequest>> MatchRequestToMe(Guid toMemberId)
        {
            return await DMDB.Set<MatchRequest>()
                .Where(mr => mr.ToId.Equals(toMemberId))
                .ToListAsync();
        }
    }
}
