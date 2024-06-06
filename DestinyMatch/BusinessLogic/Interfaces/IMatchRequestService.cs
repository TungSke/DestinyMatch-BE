using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMatchRequestService
    {
        Task<MatchRequest?> GetMatchRequestById(Guid matchRequestId);
        Task<IEnumerable<MatchRequest>?> GetMatchRequests();
        Task<MatchRequest> Matching(MatchRequestToAdd matchRequestToAdd);
        Task<List<MatchRequest>> MatchRequestOfMe(Guid memberId);
        Task<List<MatchRequest>> MatchRequestToMe(Guid memberId);
        Task<MatchRequest> MatchResponse(Guid matchRequestId, string response);
        Task<bool> RemoveMatchRequest(Guid matchRequestId);
    }
}
