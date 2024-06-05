using BusinessLogic.Models.Request;
using BusinessLogic.Models.Response;
using Repository.Models;

namespace BusinessLogic.Interfaces
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetPackages();
        Task<Package> GetPackageById(Guid id);
        Task<bool> CreatePackageAsync(PackageRequest package);
        Task<bool> UpdatePackageAsync(PackageResponse package);
        Task<bool> DeletePackageAsync(Guid id);
    }
}
