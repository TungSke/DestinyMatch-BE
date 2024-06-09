using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using System.Security.Cryptography;//For hash password
using System.Text;

namespace FPT.DestinyMatch.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        //--------------------------[ IMPLEMENT ]--------------------------
        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            if (email is null)
            {
                return null;
            }
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<bool> CreateAccountAsync(string email, string password)
        {
            var existAccount = await GetAccountByEmailAsync(email);
            if (existAccount is not null)
            {
                return false;
            }
            string hashedPassword = HashString(password);
            _accountRepository.Add(
                new Account
                {
                    Email = email,
                    Password = hashedPassword
                }
            );
            await _accountRepository.SaveChangeAsync();
            return true;
        }

        private string HashString(string input)//SHA-256 Algorithm (1 way)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<Account?> LoginByPassword(string email, string password)
        {
            var existAccount = await _accountRepository.GetByEmailAsync(email);
            if (existAccount is null || existAccount.Status.Equals("deleted") || existAccount.Status.Equals("banned"))
            {
                return null;
            }
            string hashedPassword = HashString(password);

            if (existAccount.Password.Equals(hashedPassword))
            {
                return existAccount;
            }
            return null;
        }

        public async Task<bool> ChangeRoleAccountAsync(Guid id, string newRole)
        {
            var currentAcc = await _accountRepository.GetByIdAsync(id);
            if (currentAcc == null)
            {
                return false;
            }
            currentAcc.Role = newRole;
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> ChangePasswordAccountAsync(Guid id, string oldPassword, string newPassword)
        {
            var currentAcc = await _accountRepository.GetByIdAsync(id);
            string hashedOldPassword = HashString(oldPassword);
            if (currentAcc == null || !currentAcc.Password.Equals(hashedOldPassword))
            {
                return false;
            }
            currentAcc.Password = HashString(newPassword);
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> DeleteAccountAsync(string email, string confirmPassword)
        {
            var currentAcc = await _accountRepository.GetByEmailAsync(email);
            string hashedPassword = HashString(confirmPassword);
            if (currentAcc is null || !currentAcc.Password.Equals(hashedPassword))
            {
                return false;
            }
            currentAcc.Status = "deleted";
            return await _accountRepository.SaveChangeAsync();
        }
        
        public async Task<bool> RecoverAccountAsync(string email, string newStatus)
        {
            var accFound = await _accountRepository.GetByEmailAsync(email);
            if(accFound is null)
            {
                return false;
            }
            accFound.Status = newStatus;
            return await _accountRepository.SaveChangeAsync();
        }
    }
}
