using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Services
{
    public class HobbyService : IHobbyService
    {
        private readonly IHobbyReposiroty _hobbyRepository; 

        public HobbyService(IHobbyReposiroty hobbyRepository)
        {
            _hobbyRepository = hobbyRepository;
        }

        public async Task<(IEnumerable<Hobby> hobbies, int totalCount)> GetHobbies(string search, int page , int pagesize) => await _hobbyRepository.GetHobbies(search, page , pagesize);

        public async Task<Hobby?> GetHobbyById(Guid id) => await _hobbyRepository.GetByIdAsync(id);

        public async Task<Hobby> CreateHobby(HobbyRequest hobbyRequest)
        {
            var hobbyToAdd = new Hobby
            {
                Id = new Guid(),
                Name = hobbyRequest.Name,
                Description = hobbyRequest.Description
            };
            await _hobbyRepository.Add(hobbyToAdd);
            await _hobbyRepository.SaveChangeAsync();
            return hobbyToAdd;
        }

        public async Task<Hobby> EditHobby(Guid id, HobbyRequest hobbyRequest)
        {
            var hobby = await _hobbyRepository.GetByIdAsync(id);
            if (hobby is null)
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
            if (hobby is null)
            {
                return false;
            }
            _hobbyRepository.Remove(hobby);
            await _hobbyRepository.SaveChangeAsync();
            return true;
        }
    }
}
