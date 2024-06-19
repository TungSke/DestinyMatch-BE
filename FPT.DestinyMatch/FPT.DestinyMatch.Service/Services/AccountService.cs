using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using System.Security.Cryptography;//For hash password
using System.Text;
using FPT.DestinyMatch.Service.Extensions.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client;

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
        public async Task<Account?> GetAccountByIdAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new BadRequestException("None account id like that");
            }
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new BadRequestException("Cannot find account with null email!");
            }
            return await _accountRepository.GetByEmailAsync(email);
        }
        public async Task<IEnumerable<Account>> GetAccountsListAsync(int size, int page,
            string? keyword, bool byDate, string? status, string? role, bool isDescending)
        {
            size = size == 0 ? 10 : size;
            page = page == 0 ? 1 : page;
            var accountList = await _accountRepository.GetListAsync(size, page, keyword, byDate, status, role, isDescending);
            if(accountList.Any()==false)
            {
                throw new NotFoundException("Not found any account");
            }
            return accountList;
        }

        public async Task<bool> CreateAccountAsync(string email, string password)
        {
            var existAccount = await GetAccountByEmailAsync(email);
            if (existAccount is not null)
            {
                throw new ConflictException("Account existed!");
            }
            string hashedPassword = HashString(password);
            await _accountRepository.Add(
                new Account
                {
                    Email = email,
                    Password = hashedPassword
                }
            );
            return await _accountRepository.SaveChangeAsync();
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

        public async Task<Account> LoginByPasswordAsync(string email, string password)
        {
            var existAccount = await _accountRepository.GetByEmailAsync(email);
            if (existAccount is null
                || existAccount.Status!.ToLower().Equals("deleted")
                || existAccount.Status.ToLower().Equals("banned"))
            {
                throw new BadRequestException("Email is not existed or not available");
            }
            string hashedPassword = HashString(password);

            if (!existAccount.Password!.Equals(hashedPassword))
            {
                throw new BadRequestException("Incorrect password!");
            }
            return existAccount;
        }

        public async Task<bool> ChangeRoleAccountAsync(Guid accountId, string newRole)
        {
            var currentAcc = await _accountRepository.GetByIdAsync(accountId);
            if (currentAcc is null)
            {
                return false;
            }
            currentAcc.Role = newRole;
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> ChangePasswordAccountAsync(Guid accountId, string oldPassword, string newPassword, bool privilegedOverride)
        {
            var currentAcc = await _accountRepository.GetByIdAsync(accountId);
            if (currentAcc is null)
            {
                throw new NotFoundException("Not found that account");
            }
            if (privilegedOverride == false)
            {
                string hashedOldPassword = HashString(oldPassword);
                if (!currentAcc.Password.Equals(hashedOldPassword))
                {
                    throw new BadRequestException("Wrong password!");
                }
            }
            currentAcc.Password = HashString(newPassword);
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> DeleteAccountAsync(string email, string confirmPassword, bool privilegedOverride)
        {
            var currentAcc = await _accountRepository.GetByEmailAsync(email);
            if (currentAcc is null)
            {
                throw new NotFoundException("Cannot found that email account");
            }
            if (privilegedOverride == false)
            {
                string hashedPassword = HashString(confirmPassword);
                if (!currentAcc.Password.Equals(hashedPassword))
                {
                    throw new BadRequestException("Wrong confirm password!");
                }
            }
            currentAcc.Status = "deleted";
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> RecoverAccountAsync(string email, string newStatus)
        {
            var accFound = await _accountRepository.GetByEmailAsync(email);
            if (accFound is null)
            {
                return false;
            }
            accFound.Status = newStatus;
            return await _accountRepository.SaveChangeAsync();
        }
    }
}
