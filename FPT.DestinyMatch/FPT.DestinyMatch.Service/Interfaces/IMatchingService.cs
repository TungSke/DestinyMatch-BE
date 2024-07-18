using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMatchingService
    {
        Task<IEnumerable<MatchingResponse>> GetMatchings(Guid currentMemId, int pageIndex, int pageSize, string search, string status);
        Task CreateMatching(MatchingRequest matchingrequest);
        Task DeleteMatching(Guid matchid);
        Task ChangeStatusMatching(Guid matchid, string status);
    }
}
