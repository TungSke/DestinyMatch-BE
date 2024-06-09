using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        Task<IEnumerable<University>> GetUniversities(int pageIndex, int PageSize, string searchString);
    }
}
