using Repository.DTOs.University;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
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
