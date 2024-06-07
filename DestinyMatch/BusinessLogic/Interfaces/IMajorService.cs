using BusinessLogic.Models.Request;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
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
