using Repository.DTOs.University;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IUniversitityService
    {
        Task<IEnumerable<University>> GetUniversities();
        Task<University> GetUniversityById(Guid id);
        Task<University> AddUniversity(GetUniversity university);
        Task<University> UpdateUniversity(UpdateUni university);
        Task DeleteUniversity(Guid id);
    }
}
