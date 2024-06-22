using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Repository.Interfaces
{
    public interface IVerificationRepository : IGenericRepository<Verification>
    {
        public Task<Verification?> GetDetailAsync(Guid verificationId);
        public Task<IEnumerable<Verification>> GetListVerificationAsync(
            int amountItem, int pageIndex,
            Guid memberId,
            string? statusSearch,
            bool OrderByAscending);
    }
}
