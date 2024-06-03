using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        public Task<Account> GetAccountByIdAsync(Guid id);
        public Task<Account> GetAccountByEmailAsync(string email);
        public Task<bool> CreateAccountAsync(string email, string password);
        public Task<Account> LoginByPassword(string email, string password);
        public Task<bool> ChangeRoleAccountAsync(Guid id, string newRole);
        public Task<bool> ChangePasswordAccountAsync(Guid id, string oldPassword, string newPassword);
        public Task<bool> DeleteAccountAsync(string email, string confirmPassword);
        public Task<bool> RecoverAccountAsync(string email, string newStatus);
    }
}
