using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        public Task<(IEnumerable<University> universities, int totalCount)> GetUniversities(string? search, int page, int pagesize);
    }
}
