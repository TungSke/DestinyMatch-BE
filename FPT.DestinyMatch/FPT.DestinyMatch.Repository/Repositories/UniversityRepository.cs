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

        public async Task<IEnumerable<University>> GetUniversities(int pageIndex, int PageSize, string searchString)
        {
            var list = GetAsync().AsNoTracking();
            if (!string.IsNullOrEmpty(searchString))
            {
                var search = searchString.ToLower();
                list = list.Where(x => x.Name.ToLower().Contains(search) || x.Code.ToLower().Contains(search));
            }
            if (pageIndex == 0) pageIndex = 1;
            if (PageSize == 0) PageSize = 10;
            list = list.Skip((pageIndex - 1) * PageSize).Take(PageSize);
            return list;
        }
    }
}
