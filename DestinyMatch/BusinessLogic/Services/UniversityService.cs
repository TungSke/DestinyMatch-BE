using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using BusinessLogic.Models.Response;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;

namespace BusinessLogic.Services
{
    public class UniversityService : IUniversitityService
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityService(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
        }

        public async Task<IEnumerable<University>> GetUniversities()
        {
            return await _universityRepository.Get().ToListAsync();
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
            university.Adapt(univer);
            await _universityRepository.SaveChangeAsync();
            return univer;
        }

        public async Task DeleteUniversity(Guid id)
        {
            var university = await _universityRepository.GetByIdAsync(id);
            if(university != null)
            {
                _universityRepository.Remove(university);
            }        
            await _universityRepository.SaveChangeAsync();
            return;
        }
    }
}
