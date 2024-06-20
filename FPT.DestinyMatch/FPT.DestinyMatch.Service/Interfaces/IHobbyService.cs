using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;
using Azure;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IHobbyService
    {
        Task<(IEnumerable<Hobby> hobbies, int totalCount)> GetHobbies(string search,int page, int pagesize);
        Task<Hobby?> GetHobbyById(Guid id);
        Task<Hobby> CreateHobby(HobbyRequest hobbyRequest);
        Task<Hobby> EditHobby(Guid id, HobbyRequest hobbyRequest);
        Task<bool> DeleteHobby(Guid id);
    }
}
