using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using BusinessLogic.Models.Response;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;

namespace BusinessLogic.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public async Task<IEnumerable<Package>> GetPackages()
        {
            
            return await _packageRepository.Get().ToListAsync(); 
        }
        
        public async Task<Package> GetPackageById(Guid id)
        {
            return await _packageRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreatePackageAsync(PackageRequest package)
        {
            var existed = await _packageRepository.Get().AnyAsync(x => x.Code.ToLower().Equals(package.Code.ToLower()));
            if(existed == true)
            {
                return false;
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
            if(existed == null)
            {
                return false;
            }
            else
            {
                var mapster = package.Adapt(existed);
                await _packageRepository.SaveChangeAsync();
                return true;
            }
        }

        public async Task<bool> DeletePackageAsync(Guid id)
        {
            var existed = await _packageRepository.GetByIdAsync(id);
            if(existed == null)
            {
                return false;
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
