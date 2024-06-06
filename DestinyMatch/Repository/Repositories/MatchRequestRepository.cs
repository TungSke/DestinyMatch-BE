using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
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
