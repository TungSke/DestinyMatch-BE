using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IHobbyService
    {
        Task<IEnumerable<Hobby>> GetHobbies();
        Task<Hobby?> GetHobbyById(Guid id);
        Task<Hobby> CreateHobby(HobbyRequest hobbyRequest);
        Task<Hobby> EditHobby(Guid id, HobbyRequest hobbyRequest);
        Task<bool> DeleteHobby(Guid id);
    }
}
