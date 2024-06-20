using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMajorService
    {
        public Task<(IEnumerable<Major> majors, int totalCount)> GetMajors(string search, int page, int pagesize);
        Task<Major?> GetMajorById(Guid id);
        Task<Major> CreateMajor(MajorRequest majorRequest);
        Task<Major> EditMajor(Guid id, MajorRequest majorRequest);
        Task<bool> DeleteMajor(Guid id);
    }
}
