using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Service.Models.Response;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.API.Models.ResponseModels;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IUniversitityService
    {
        Task<PageModel<University>> GetUniversities(int pageIndex, int PageSize, string searchString);
        Task<University> GetUniversityById(Guid id);
        Task<University> AddUniversity(UniversityRequest university);
        Task<University> UpdateUniversity(UniversityResponse university);
        Task DeleteUniversity(Guid id);
    }
}
