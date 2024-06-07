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
    public class HobbyService : IHobbyService
    {
        private readonly IHobbyReposiroty _hobbyRepository; 

        public HobbyService(IHobbyReposiroty hobbyRepository)
        {
            _hobbyRepository = hobbyRepository;
        }

        public async Task<IEnumerable<Hobby>> GetHobbies() => await _hobbyRepository.Get().ToListAsync();

        public async Task<Hobby?> GetHobbyById(Guid id) => await _hobbyRepository.GetByIdAsync(id);

        public async Task<Hobby> CreateHobby(HobbyRequest hobbyRequest)
        {
            var hobbyToAdd = new Hobby
            {
                Id = new Guid(),
                Name = hobbyRequest.Name,
                Description = hobbyRequest.Description
            };
            _hobbyRepository.Add(hobbyToAdd);
            await _hobbyRepository.SaveChangeAsync();
            return hobbyToAdd;
        }

        public async Task<Hobby> EditHobby(Guid id, HobbyRequest hobbyRequest)
        {
            var hobby = await _hobbyRepository.GetByIdAsync(id);
            if (hobby == null)
            {
                return null;
            }
            hobby.Name = hobbyRequest.Name ?? hobby.Name;
            hobby.Description = hobbyRequest.Description ?? hobby.Description;
            await _hobbyRepository.SaveChangeAsync();
            return hobby;
        }

        public async Task<bool> DeleteHobby(Guid id) 
        {
            var hobby = await _hobbyRepository.GetByIdAsync(id);
            if (hobby == null)
            {
                return false;
            }
            _hobbyRepository.Remove(hobby);
            await _hobbyRepository.SaveChangeAsync();
            return true;
        }
    }
}
