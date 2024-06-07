using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IMatchRequestRepository : IGenericRepository<MatchRequest>
    {
        Task<List<MatchRequest>> MatchRequestOfMe(Guid fromMemberId);

        Task<List<MatchRequest>> MatchRequestToMe(Guid toMemberId);
    }
}
