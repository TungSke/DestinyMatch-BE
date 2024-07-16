using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IHobbyReposiroty : IGenericRepository<Hobby>
    {
        public Task<(IEnumerable<Hobby> hobbies, int totalCount)> GetHobbies(string? search, int page, int pagesize);
    }
}