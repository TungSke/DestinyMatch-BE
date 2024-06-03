using BusinessLogic.Interfaces;
using BusinessLogic.Models.Request;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using Repository.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class MajorService : IMajorService
    {
        private readonly IMajorRepository _majorRepository;

        public MajorService(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public async Task<IEnumerable<Major>> GetAllMajor() => await _majorRepository.GetAllAsync().ToListAsync();

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
            if (major == null)
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
            if (major == null)
            {
                return false;
            }
            _majorRepository.Remove(major);
            await _majorRepository.SaveChangeAsync();
            return true;
        }
    }
}
