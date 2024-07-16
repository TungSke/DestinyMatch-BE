using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IPackageService
    {
        Task<PageModel<Package>> GetPackages(int pageIndex, int PageSize, string searchString);
        Task<Package> GetPackageById(Guid id);
        Task<bool> CreatePackageAsync(PackageRequest package);
        Task<bool> UpdatePackageAsync(PackageResponse package);
        Task<bool> DeletePackageAsync(Guid id);
    }
}
