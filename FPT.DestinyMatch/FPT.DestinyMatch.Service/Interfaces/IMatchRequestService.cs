using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
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
