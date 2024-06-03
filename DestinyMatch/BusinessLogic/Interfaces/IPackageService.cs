using Repository.DTOs.Package;
using Repository.Models;

namespace BusinessLogic.Interfaces
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetPackages();
        Task<Package> GetPackageById(Guid id);
        Task<bool> CreatePackageAsync(CreatePackage package);
        Task<bool> UpdatePackageAsync(UpdatePackage package);
        Task<bool> DeletePackageAsync(Guid id);
    }
}
