using FPT.DestinyMatch.Repository.Models;

namespace FPT.DestinyMatch.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<Account> GetAccountByIdAsync(Guid accountId);
        public Task<Account> GetAccountByEmailAsync(string email);
        public Task<IEnumerable<Account>> GetAccountsListAsync(int size, int page,
            string? keyword, bool byDate, string? status, string? role, bool isDescending);
        public Task<bool> CreateAccountAsync(string email, string password);
        public Task<Account> LoginByPasswordAsync(string email, string password);
        public Task<bool> ChangeRoleAccountAsync(Guid accountId, string newRole);
        public Task<bool> ChangePasswordAccountAsync(Guid accountId, string oldPassword, string newPassword, bool privilegedOverride);
        public Task<bool> DeleteAccountAsync(string email, string confirmPassword, bool privilegedOverride);
        public Task<bool> RecoverAccountAsync(string email, string newStatus);
    }
}
