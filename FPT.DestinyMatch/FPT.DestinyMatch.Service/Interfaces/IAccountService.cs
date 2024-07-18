using FPT.DestinyMatch.Repository.Models;
using Microsoft.AspNetCore.Http;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<Account> GetAccountByIdAsync(Guid accountId);
        public Task<Account> GetMemberByAccountId(Guid accountId);
        public Task<IEnumerable<Account>> GetAccountsListAsync(int size, int page,
            string? keyword, bool byDate, string? status, string? role, bool isDescending);
        public Task<bool> CreateAccountAsync(string email, string password, bool ReceiveEmail);
        public Task<string> LoginByPasswordAsync(string email, string password);
        public Task<string> HandleGoogleAsync(string token, string platform);
        public Task<bool> ChangeRoleAccountAsync(Guid accountId, string newRole);
        public Task<bool> ChangePasswordAccountAsync(Guid accountId, string oldPassword, string newPassword, bool privilegedOverride);
        public Task<bool> DeleteAccountAsync(Guid accountId, string confirmPassword);
        public Task<bool> RecoverAccountAsync(string email, string newStatus);
        public Task<bool> BanAccount(Guid accountId);
        Task UpdateFcmToken(HttpContext User, string fcmtoken);
    }
}
