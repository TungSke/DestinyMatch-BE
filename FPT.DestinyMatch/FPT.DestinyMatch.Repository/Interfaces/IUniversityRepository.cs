using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        public Task<IEnumerable<University>> GetUniversities(int page, int pagesize, string? search);
    }
}
