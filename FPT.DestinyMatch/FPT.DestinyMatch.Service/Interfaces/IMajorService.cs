using FPT.DestinyMatch.Service.Models.Request;
using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IMajorService
    {
        Task<IEnumerable<Major>> GetAllMajor();
        Task<Major?> GetMajorById(Guid id);
        Task<Major> CreateMajor(MajorRequest majorRequest);
        Task<Major> EditMajor(Guid id, MajorRequest majorRequest);
        Task<bool> DeleteMajor(Guid id);
    }
}
