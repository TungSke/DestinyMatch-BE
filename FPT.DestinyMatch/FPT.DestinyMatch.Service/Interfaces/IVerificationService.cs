using FPT.DestinyMatch.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IVerificationService
    {
        public Task<Verification> GetVerificationDetailAsync(Guid verificationId);
        public Task<IEnumerable<Verification>> GetListVerificationAsync(
            int amount, int page,
            Guid MemberId,
            string? Status,
            bool OrderByAscending);
        public Task<bool> CreateVerificationAsync(string? submittedPicture, Guid memberId);
        public Task<bool> UpdateStatusVerificationAsync(Guid verificationId, string newStatus);
        public Task<bool> DeleteVerificationAsync(Guid verificationId);
    }
}
