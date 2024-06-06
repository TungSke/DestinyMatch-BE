using Repository.DTOs.Package;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
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
