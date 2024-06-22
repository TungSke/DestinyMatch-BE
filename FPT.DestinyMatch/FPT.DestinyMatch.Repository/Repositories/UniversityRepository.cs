using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        public UniversityRepository(DestinyMatchContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<University> universities, int totalCount)> GetUniversities(string? search, int page, int pagesize)
        {
            var universities = DMDB.Universities.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                universities = universities.Where(u => u.Name.Contains(search) || u.Code.Contains(search));
            }

            var totalCount = await universities.CountAsync();
            universities = universities.Skip((page - 1) * pagesize).Take(pagesize);

            return (universities, totalCount);
        }
    }
}
