using FPT.DestinyMatch.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;

namespace FPT.DestinyMatch.Service.Services
{
    public class UniversityService : IUniversitityService
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityService(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        public async Task<IEnumerable<University>> GetUniversities(int pageIndex, int PageSize, string searchString)
        {
            return await _universityRepository.GetUniversities(pageIndex,PageSize,searchString);
        }

        public async Task<University> GetUniversityById(Guid id)
        {
            return await _universityRepository.GetByIdAsync(id);
        }

        public async Task<University> AddUniversity(UniversityRequest university)
        {
            var u = university.Adapt<University>();
            _universityRepository.Add(u);
            await _universityRepository.SaveChangeAsync();
            return u;
        }

        public async Task<University> UpdateUniversity(UniversityResponse university)
        {
            var univer = await _universityRepository.GetByIdAsync(university.Id);
            if (univer == null)
            {
                throw new Exception("University not found");
            }
            university.Adapt(univer);
            await _universityRepository.SaveChangeAsync();
            return univer;
        }

        public async Task DeleteUniversity(Guid id)
        {
            var university = await _universityRepository.GetByIdAsync(id);
            if (university != null)
            {
                _universityRepository.Remove(university);
            }
            else throw new Exception("University not found");
            await _universityRepository.SaveChangeAsync();
            return;
        }
    }
}
