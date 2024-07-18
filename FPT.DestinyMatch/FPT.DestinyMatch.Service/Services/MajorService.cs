using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Services
{
    public class MajorService : IMajorService
    {
        private readonly IMajorRepository _majorRepository;

        public MajorService(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public async Task<(IEnumerable<Major> majors, int totalCount)> GetMajors(string search, int page, int pagesize) => await _majorRepository.GetMajors(search, page, pagesize);

        public async Task<Major?> GetMajorById(Guid id) => await _majorRepository.GetByIdAsync(id);

        public async Task<Major> CreateMajor(MajorRequest majorRequest)
        {
            var MajorToAdd = new Major
            {
                Id = new Guid(),
                Code = majorRequest.Code,
                Name = majorRequest.Name,
            };
            _majorRepository.Add(MajorToAdd);
            await _majorRepository.SaveChangeAsync();
            return MajorToAdd;
        }

        public async Task<Major> EditMajor(Guid id, MajorRequest majorRequest)
        {
            var major = await _majorRepository.GetByIdAsync(id);
            if (major is null)
            {
                return null;
            }
            major.Code = majorRequest.Code ?? major.Code;
            major.Name = majorRequest.Name ?? major.Name;
            await _majorRepository.SaveChangeAsync();
            return major;
        }

        public async Task<bool> DeleteMajor(Guid id)
        {
            var major = await _majorRepository.GetByIdAsync(id);
            if (major is null)
            {
                return false;
            }
            _majorRepository.Remove(major);
            await _majorRepository.SaveChangeAsync();
            return true;
        }
    }
}
