using BusinessLogic.Models.Request;
using BusinessLogic.Models.Response;
using Repository.Models;

namespace BusinessLogic.Interfaces
{
    public interface IUniversitityService
    {
        Task<IEnumerable<University>> GetUniversities();
        Task<University> GetUniversityById(Guid id);
        Task<University> AddUniversity(UniversityRequest university);
        Task<University> UpdateUniversity(UniversityResponse university);
        Task DeleteUniversity(Guid id);
    }
}
