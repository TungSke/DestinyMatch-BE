using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Models;
using System.Security.Cryptography;//For hash password
using System.Text;
using FPT.DestinyMatch.Service.Extensions.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FPT.DestinyMatch.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _config;
        public AccountService(IAccountRepository accountRepository, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _config = config;
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

        public async Task<bool> CreateAccountAsync(string email, string password, bool ReceiveEmail)
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
                    Password = hashedPassword,
                    ReceiveNotifiEmail = ReceiveEmail,
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

        public async Task<string> LoginByPasswordAsync(string email, string password)
        {
            var foundAccount = await _accountRepository.GetValidAccountByEmail(email)
               ?? throw new BadRequestException("This account is not registered or not available");

            BannedChecker(foundAccount.Status);

            string hashedPassword = HashString(password);

            if (!foundAccount.Password!.Equals(hashedPassword))
            {
                throw new BadRequestException("Incorrect password!");
            }
            string memberid = foundAccount?.Member?.Id.ToString() ?? "";
            //==========================
            var jwtToken = GenerateToken(
                foundAccount!.Id.ToString(),
                foundAccount.Email!,
                foundAccount.Role,
                "",
                memberid
            );   
            return jwtToken;
        }
        private string GenerateToken(string id, string email, string role, string? name, string memberid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            name = name ?? "";
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),//Jwt standard claim
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),//Jwt claim in .Net
                new Claim("memberid", memberid),
                new Claim(JwtRegisteredClaimNames.Name, name),
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
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
            var currentAcc = await _accountRepository.GetByIdAsync(accountId)
                ??throw new NotFoundException("Not found that account");
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

        public async Task<string> HandleGoogleAsync(string googleToken, string platform)
        {
            var payload = await ValidateGoogleToken(googleToken, platform);
            // Extract the email from the payload
            var email = payload.Email;
            var fullname = payload.Name;

            var existAccount = await _accountRepository.GetValidAccountByEmail(email);
            string jwtToken;
            if (existAccount is null) //null -> create account with that mail
            {
                await _accountRepository.Add(new Account { Email = email });
                await _accountRepository.SaveChangeAsync();

                //then return account object
                var registered = await _accountRepository.GetValidAccountByEmail(email)
                    ?? throw new BadRequestException("There is an error while Signup this email using google!");

                jwtToken = GenerateToken(
                    registered.Id.ToString(),
                    registered.Email!,
                    registered.Role,
                    fullname,"");
                return jwtToken;
            }

            BannedChecker(existAccount!.Status);
            jwtToken = GenerateToken(
                    existAccount.Id.ToString(),
                    existAccount.Email!,
                    existAccount.Role,
                    fullname,
                    "");
            return jwtToken;
        }
        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token, string platform)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = platform.ToLower().Equals("web") ?
                new List<string> { _config["Google:web:client_id"]! } :
                new List<string> { _config["Google:mobile:client_id"]! }
            };

            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }
        private static void BannedChecker(string status)
        {
            if (status.ToLower().Equals("banned"))
            {
                throw new BadRequestException("This Account has been banned and can't not be login or signup again!");
            }
        }

        public async Task UpdateFcmToken(HttpContext User, string fcmtoken)
        {
            var accountid = User.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var account = await _accountRepository.GetAsync().FirstOrDefaultAsync(x => x.Id == Guid.Parse(accountid));
            account.FcmtToken = fcmtoken;
            await _accountRepository.SaveChangeAsync();
        }
    }
}
