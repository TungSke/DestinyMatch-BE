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
        public async Task<Account> GetAccountByIdAsync(Guid accountId)
        {
            if (accountId == Guid.Empty)
            {
                throw new BadRequestException("None account id like that");
            }
            var acc = await _accountRepository.GetByIdAsync(accountId);
            return (acc is null) ? throw new NotFoundException("Not found that account id") : acc;
        }

        public async Task<Account> GetMemberByAccountId(Guid accountId)
        {
            var acc = await _accountRepository.GetByIdIncludeMember(accountId)
                ?? throw new NotFoundException("Don't found any account suitable that Id");
            return (acc.Member is null) ? throw new NotFoundException("This account haven't create profile yet!") : acc;
        }
        public async Task<IEnumerable<Account>> GetAccountsListAsync(int size, int page,
            string? keyword, bool byDate, string? status, string? role, bool isDescending)
        {
            size = size == 0 ? 10 : size;
            page = page == 0 ? 1 : page;
            var accountList = await _accountRepository.GetListAsync(size, page, keyword, byDate, status, role, isDescending);
            if (accountList.Any() == false)
            {
                throw new NotFoundException("Not found any account");
            }
            return accountList;
        }

        public async Task<bool> CreateAccountAsync(string email, string password)
        {
            var existAccount = await _accountRepository.GetValidAccountByEmail(email);

            if (existAccount is not null)
            {
                BannedChecker(existAccount.Status);
                throw new ConflictException("Account existed! Cannot create duplicate account");
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
            var foundAccount = await _accountRepository.GetValidAccountByEmail(email);
            if (foundAccount is null)
            {
                throw new BadRequestException("This account is not registered or not available");
            }

            BannedChecker(foundAccount.Status);

            string hashedPassword = HashString(password);

            if (!foundAccount.Password!.Equals(hashedPassword))
            {
                throw new BadRequestException("Incorrect password!");
            }
            return foundAccount;
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

        public async Task<bool> DeleteAccountAsync(Guid accountId, string confirmPassword)
        {
            var currentAcc = await _accountRepository.GetByIdAsync(accountId);
            if (currentAcc is null)
            {
                throw new NotFoundException("Cannot found that account");
            }

            string hashedPassword = HashString(confirmPassword);
            if (!currentAcc.Password!.Equals(hashedPassword))
            {
                throw new BadRequestException("Wrong confirm password!");
            }
            currentAcc.Status = "deleted";
            return await _accountRepository.SaveChangeAsync();
        }
        public async Task<bool> BanAccount(Guid accountId)
        {
            var existAccount = await _accountRepository.GetByIdAsync(accountId) ?? throw new NotFoundException("Not found this account id");
            existAccount.Status = "banned";
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<bool> RecoverAccountAsync(string email, string newStatus)
        {
            var accFound = await _accountRepository.GetValidAccountByEmail(email);
            if (accFound is null)
            {
                return false;
            }
            accFound.Status = newStatus;
            return await _accountRepository.SaveChangeAsync();
        }

        public async Task<Account> HandleGoogleAsync(string email)
        {
            var existAccount = await _accountRepository.GetValidAccountByEmail(email);

            if (existAccount is null) //null -> create account with that mail
            {
                await _accountRepository.Add(new Account { Email = email });
                await _accountRepository.SaveChangeAsync();

                //then return account object
                var registered = await _accountRepository.GetValidAccountByEmail(email)
                    ?? throw new BadRequestException("There is an error while Signup this email using google!");
                return registered;
            }

            BannedChecker(existAccount!.Status);
            return existAccount;
        }
        private static void BannedChecker(string status)
        {
            if (status.ToLower().Equals("banned"))
            {
                throw new BadRequestException("This Account has been banned and can't not be login or signup again!");
            }
        }
    }
}
