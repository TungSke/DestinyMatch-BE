using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
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
