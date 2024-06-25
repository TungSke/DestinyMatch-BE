using FPT.DestinyMatch.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;
using FPT.DestinyMatch.API.Models.ResponseModels;
using FPT.DestinyMatch.Repository.Repositories;

namespace FPT.DestinyMatch.Service.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public async Task<PageModel<Package>> GetPackages(int pageIndex, int PageSize, string searchString)
        {
            var packages = await _packageRepository.GetPackages(pageIndex, PageSize, searchString);
            var totalRecords = packages.Count();
            var totalPages = totalRecords > 0 ? (int)Math.Ceiling((double)totalRecords / PageSize) : 0;
            return new PageModel<Package>
            {
                PageIndex = pageIndex,
                PageSize = PageSize,
                totalPage = totalPages,
                Count = totalRecords,
                Data = packages
            };
        }

        public async Task<Package> GetPackageById(Guid id)
        {
            return await _packageRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreatePackageAsync(PackageRequest package)
        {
            var existed = await _packageRepository.GetAsync().FirstOrDefaultAsync(x => x.Code.ToLower().Equals(package.Code.ToLower()));
            if (existed != null)
            {
                throw new Exception("Package Existed");
            }
            else
            {
                var mapster = package.Adapt<Package>();
                _packageRepository.Add(mapster);
                await _packageRepository.SaveChangeAsync();
                return true;
            }
        }

        public async Task<bool> UpdatePackageAsync(PackageResponse package)
        {
            var existed = await _packageRepository.GetByIdAsync(package.Id);
            if (existed is null)
            {
                throw new Exception("Package not found");
            }
            else
            {
                package.Adapt(existed);
                await _packageRepository.SaveChangeAsync();
                return true;
            }
        }

        public async Task<bool> DeletePackageAsync(Guid id)
        {
            var existed = await _packageRepository.GetByIdAsync(id);
            if (existed is null)
            {
                throw new Exception("Package not found");
            }
            else
            {
                _packageRepository.Remove(existed);
                await _packageRepository.SaveChangeAsync();
                return true;
            }
        }
    }
}
