using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IMatchingRepository : IGenericRepository<Matching>
    {
        Task<List<Matching>> GetMatchings(Guid usingMemid, int pageIndex, int pageSize, string search, string status);
    }
}
