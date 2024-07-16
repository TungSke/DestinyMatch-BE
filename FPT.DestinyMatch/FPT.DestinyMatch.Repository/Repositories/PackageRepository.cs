using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Repository.Repositories
{
    public class PackageRepository : GenericRepository<Package>, IPackageRepository
    {
        public PackageRepository(DestinyMatchContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Package>> GetPackages(int pageIndex, int PageSize, string searchString)
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
