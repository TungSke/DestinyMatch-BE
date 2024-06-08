using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IPackageRepository : IGenericRepository<Package>
    {
        Task<IEnumerable<Package>> GetPackages(int pageIndex, int PageSize, string searchString);
    }
}
