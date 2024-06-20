using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IMajorRepository : IGenericRepository<Major>
    {
        public Task<(IEnumerable<Major> majors, int totalCount)> GetMajors(string? search, int page, int pagesize);
    }
}
